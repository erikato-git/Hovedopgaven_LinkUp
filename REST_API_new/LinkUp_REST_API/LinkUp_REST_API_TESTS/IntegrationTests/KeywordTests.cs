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
using LinkUp_REST_API.Controllers;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Services;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;
using LinkUp_REST_API_TESTS.TestHelpers;

namespace LinkUp_REST_API_TESTS.IntegrationTests
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
            var createProfileDto = KeywordTestHelper.GenerateValidKeywordCreateUpdateInput();       // dto supposed to be associated to profile that is associated to logged in user

            var result = await _sut.CreateKeyword(createProfileDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateKeyword_Should_Return401_When_UserIsNotLoggedIn()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeywordCreateUpdateInput();       // dto supposed to be associated to profile that is associated to logged in user
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.CreateKeyword(createProfileDto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task CreateKeyword_Should_Return403_When_UserTriesToAddKeywordToAProfileItDoesntOwn()
        {
            var createProfileDto = KeywordTestHelper.GenerateValidKeywordCreateUpdateInput();       // dto supposed to be associated to profile that is associated to logged in user
            createProfileDto.ProfileId = Guid.NewGuid();

            var result = await _sut.CreateKeyword(createProfileDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }


        // GetKeywordById

        [Fact]
        public async Task GetKeywordById_Should_Return200_When_OneOfLoggedInUserProfilesMatchWithKeywordId()
        {
            var validId = KeywordTestHelper.GetValidKeywordId1();   // logged in account supposed to have profile that is supposed to have this keyword-id

            var result = await _sut.GetKeywordById(validId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
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
