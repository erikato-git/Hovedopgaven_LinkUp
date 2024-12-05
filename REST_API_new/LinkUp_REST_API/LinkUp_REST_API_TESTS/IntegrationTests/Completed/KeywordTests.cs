using AutoFixture;
using LinkUp_REST_API.Controllers.Completed;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Repositories.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
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
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Services;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;

namespace LinkUp_REST_API_TESTS.IntegrationTests.Completed
{
    [Collection("Shared collection")]
    public class KeywordTests : IAsyncLifetime
    {

        private ApplicationFactory _factory;
        private Fixture _fixture;
        private IServiceScope _scope;

        // reals

        private KeywordsController _sut;

        private IKeywordService _keywordService;
        private IKeywordRepository _keywordRepository;
        private IProfileRepository _profileRepository;
        private IAuthentication _authentication;
        private IAccountRepository _accountRepository;
        private DataContext _dbContext;
        private IOptions<JwtSettings> _jwtSettings;

        public KeywordTests(ApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            // reals

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
            _jwtSettings = _scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();

            _keywordRepository = new KeywordRepository(_dbContext);
            _authentication = new Authentication(_jwtSettings);
            _accountRepository = new AccountRepository(_dbContext);
            _profileRepository = new ProfileRepository(_dbContext);

            // Config reals / mocks

            _keywordService = new KeywordService(_accountRepository, _keywordRepository, _profileRepository);

            _sut = new KeywordsController(_keywordService, _authentication)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = AuthenticationTestHelper.GetClaimsPrincipal() }
                }
            };
        }

        // CreateKeyword
        [Fact]
        public async Task CreateKeyword_Should_Return201_When_ProfileHasBeenCreatedForLoggedInAccount()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeywordToProfileWihoutKeyword();       // dto supposed to be associated to profile that has no keyword

            var result = await _sut.CreateKeyword(createProfileDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateKeyword_Should_Return409_When_UserTriesToAddANewKeywordToAProfileThatAlreadyHasAKeyword()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeyword();       // dto supposed to be associated to profile that is associated to logged in user

            var result = await _sut.CreateKeyword(createProfileDto);

            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }

        [Fact]
        public async Task CreateKeyword_Should_Return401_When_UserIsNotLoggedIn()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeyword();       // dto supposed to be associated to profile that is associated to logged in user
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.CreateKeyword(createProfileDto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task CreateKeyword_Should_Return403_When_UserTriesToAddKeywordToAProfileItDoesntOwn()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeyword();       // dto supposed to be associated to profile that is associated to logged in user
            createProfileDto.ProfileId = Guid.NewGuid();

            var result = await _sut.CreateKeyword(createProfileDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }


        // GetKeywordById

        [Fact]
        public async Task GetKeywordById_Should_Return200_When_OneOfLoggedInUserProfilesMatchWithKeywordId()
        {
            var validId = KeywordTestHelper.GetValidKeywordId1();

            var result = await _sut.GetKeywordById(validId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetKeywordById_Should_Return403_When_OneOfLoggedInUserProfilesDontMatchWithKeywordId()
        {
            var result = await _sut.GetKeywordById(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetKeywordById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var validId = KeywordTestHelper.GetValidKeywordId1();   // logged in account supposed to have profile that is supposed to have this keyword-id
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.GetKeywordById(validId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        // UpdateKeyword

        [Fact]
        public async Task UpdateKeyword_Should_Return200_When_KeywordHasBeenUpdatedWithDetailsFromValidDTO()
        {
            var updateKeyword = KeywordTestHelper.GenerateValidKeywordUpdateInput();        // KeywordId is supposed to match with keyword-id in a profile in logged in account

            var result = await _sut.UpdateKeyword(updateKeyword);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateKeyword_Should_Return401_When_UserIsNotLoggedIn()
        {
            var updateKeyword = KeywordTestHelper.GenerateValidKeywordUpdateInput();        // KeywordId is supposed to match with keyword-id in a profile in logged in account
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.UpdateKeyword(updateKeyword);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task UpdateKeyword_Should_Return403_When_KeywordHasBeenUpdatedWithDetailsFromInValidDTO()
        {
            var updateKeyword = KeywordTestHelper.GenerateValidKeywordUpdateInput();        // KeywordId is supposed to match with keyword-id in a profile in logged in account
            updateKeyword.KeywordId = Guid.NewGuid();

            var result = await _sut.UpdateKeyword(updateKeyword);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        // DeleteKeyword

        [Fact]
        public async Task DeleteKeyword_Should_Return204_When_KeywordIdIsValid()
        {
            var validId = KeywordTestHelper.GetValidKeywordId1(); // supposed to have a keyword associated to a profile associtated to logged in account

            var result = await _sut.DeleteKeywordById(validId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteKeyword_Should_Return401_When_UserIsNotLoggedIn()
        {
            var validId = KeywordTestHelper.GetValidKeywordId1(); // supposed to have a keyword associated to a profile associtated to logged in account
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.DeleteKeywordById(validId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task DeleteKeyword_Should_Return404_When_KeywordDoesNotExist()
        {
            var result = await _sut.DeleteKeywordById(Guid.NewGuid());

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteKeyword_Should_Return403_When_KeywordExistButDoentBelongToLoggedInAccount()
        {
            var invalidId = KeywordTestHelper.GetValidKeywordId2();

            var result = await _sut.DeleteKeywordById(invalidId);

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
