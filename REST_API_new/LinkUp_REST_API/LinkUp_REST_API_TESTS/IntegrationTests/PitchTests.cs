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
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using LinkUp_REST_API_TESTS.TestHelpers;
using LinkUp_REST_API_TESTS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using REST_API.Repositories.Interfaces;
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
