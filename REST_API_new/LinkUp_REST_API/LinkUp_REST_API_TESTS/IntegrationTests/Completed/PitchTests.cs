using AutoFixture;
using LinkUp_REST_API.Controllers;
using LinkUp_REST_API.Controllers.Completed;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Repositories.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services;
using LinkUp_REST_API.Services.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;
using LinkUp_REST_API_TESTS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkUp_REST_API_TESTS.IntegrationTests
{
    [Collection("Shared collection")]
    public class PitchTests : IAsyncLifetime
    {
        private ApplicationFactory _factory;
        private Fixture _fixture;
        private IServiceScope _scope;

        // reals

        private PitchesController _sut;

        private IPitchService _pitchService;
        private IAuthentication _authentication;
        private IPitchRepository _pitchRepository;
        private IAccountRepository _accountRepository;
        private IProfileRepository _profileRepository;
        private DataContext _dbContext;
        private IOptions<JwtSettings> _jwtSettings;
        private IOptions<CloudinarySettings> _cloudinary;

        public PitchTests(ApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            // reals

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
            _jwtSettings = _scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();
            _cloudinary = _scope.ServiceProvider.GetRequiredService<IOptions<CloudinarySettings>>();

            _pitchRepository = new PitchRepository(_dbContext);
            _authentication = new Authentication(_jwtSettings);
            _accountRepository = new AccountRepository(_dbContext);
            _profileRepository = new ProfileRepository(_dbContext);

            // Config reals / mocks

            _pitchService = new PitchService(_accountRepository, _pitchRepository, _profileRepository);

            _sut = new PitchesController(_pitchService, _authentication)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = AuthenticationTestHelper.GetClaimsPrincipal() }
                }
            };
        }


        // CreateProfile

        [Fact]
        public async Task CreateProfile_Should_Return201_When_ProfileHasBeenCreatedForLoggedInAccount()
        {
            var createPitchDto = PitchTestHelper.GenerateValidPitchCreateInput();

            var result = await _sut.CreatePitch(createPitchDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateProfile_Should_Return401_When_UserIsNotLoggedIn()
        {
            var createPitchDto = PitchTestHelper.GenerateValidPitchCreateInput();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.CreatePitch(createPitchDto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task CreateProfile_Should_Return409_When_LoggedInAccountDoesNotContainSendingProfile()
        {
            var createPitchDto = PitchTestHelper.GenerateValidPitchCreateInput();
            createPitchDto.SenderProfileId = Guid.NewGuid();

            var result = await _sut.CreatePitch(createPitchDto);

            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }

        [Fact]
        public async Task CreateProfile_Should_Return409_When_LoggedInAccountContainsRecipientProfile()
        {
            var createPitchDto = PitchTestHelper.GenerateValidPitchCreateInput();
            createPitchDto.RecipientProfileId = ProfileTestHelper.GetValidProfileId1();     // logged in account is supposed to contain this profileId 

            var result = await _sut.CreatePitch(createPitchDto);

            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }

        [Fact]
        public async Task CreateProfile_Should_Return404_When_RecipientProfileDoesNotExist()
        {
            var createPitchDto = PitchTestHelper.GenerateValidPitchCreateInput();
            createPitchDto.RecipientProfileId = Guid.NewGuid();

            var result = await _sut.CreatePitch(createPitchDto);

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        // GetAllAssociatedPitches

        [Fact]
        public async Task GetAllAssociatedPitches_Should_Return200_When_AssociatedPitchesForLoggedInAccountHaveBeenFetched()
        {

            var result = await _sut.GetAllAssociatedPitches();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAssociatedPitches_Should_Return401_When_UserIsNotLoggedIn()
        {
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.GetAllAssociatedPitches();

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        // GetPitchById

        [Fact]
        public async Task GetPitchById_Should_Return200_When_UserIsAssociatedAndPitchIsFound()
        {
            var validPitchId = PitchTestHelper.GetValidPitchId1();  // supposed to be associated to logged in user

            var result = await _sut.GetPitchById(validPitchId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPitchById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var validPitchId = PitchTestHelper.GetValidPitchId1();  
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.GetPitchById(validPitchId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task GetPitchById_Should_Return404_When_PitchIsNotFound()
        {
            var result = await _sut.GetPitchById(Guid.NewGuid());

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPitchById_Should_Return403_When_LoggedInUserIsNotAssociatedWithPitchId()
        {
            var notAssociatedId = PitchTestHelper.GetPitchIdWithNoAssociated();

            var result = await _sut.GetPitchById(notAssociatedId);

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        // DeletePitchById

        [Fact]
        public async Task DeletePitchById_Should_Return204_When_UserIsAssociatedAndPitchIsFound()
        {
            var validPitchId = PitchTestHelper.GetValidPitchId2();  // supposed to have logged in user as sending profile

            var result = await _sut.DeletePitchById(validPitchId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeletePitchById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var validPitchId = PitchTestHelper.GetValidPitchId2();  // supposed to have logged in user as sending profile
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.DeletePitchById(validPitchId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task DeletePitchById_Should_Return403_When_LoggedInUserDoesNotContainSendingProfile()
        {
            var validPitchId = PitchTestHelper.GetValidPitchId1();  // not supposed to have logged in user as sending profile

            var result = await _sut.DeletePitchById(validPitchId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }




        public Task DisposeAsync()
        {
            DbInitializerForTests.ReinitDbForTests(_dbContext);
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
