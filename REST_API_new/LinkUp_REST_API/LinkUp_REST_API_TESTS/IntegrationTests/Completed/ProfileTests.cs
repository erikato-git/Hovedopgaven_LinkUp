using AutoFixture;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Services;
using LinkUp_REST_API.Util;
using LinkUp_REST_API_TESTS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using LinkUp_REST_API.Repositories.Completed;
using LinkUp_REST_API.Controllers.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Completed;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;

namespace LinkUp_REST_API_TESTS.IntegrationTests.Completed
{
    [Collection("Shared collection")]
    public class ProfileTests : IAsyncLifetime
    {
        private ApplicationFactory _factory;
        private Fixture _fixture;
        private IServiceScope _scope;

        // reals

        private ProfilesController _sut;

        private IProfileService _profileService;
        private IProfileServiceHelper _profileServiceHelper;
        private IAuthentication _authentication;
        private IProfileRepository _profileRepository;
        private IAccountRepository _accountRepository;
        private DataContext _dbContext;
        private IOptions<JwtSettings> _jwtSettings;
        private IOptions<CloudinarySettings> _cloudinary;

        public ProfileTests(ApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            // reals

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
            _jwtSettings = _scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();
            _cloudinary = _scope.ServiceProvider.GetRequiredService<IOptions<CloudinarySettings>>();

            _profileRepository = new ProfileRepository(_dbContext);
            _authentication = new Authentication(_jwtSettings);
            _profileServiceHelper = new ProfileServiceHelper(_dbContext, _cloudinary);
            _accountRepository = new AccountRepository(_dbContext);

            // Config reals / mocks

            _profileService = new ProfileService(_accountRepository, _profileRepository, _profileServiceHelper);

            _sut = new ProfilesController(_profileService, _authentication)
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
            var createProfileDto = ProfileTestHelper.GenerateValidProfileCreateInput();

            var result = await _sut.CreateProfile(createProfileDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }


        [Fact]
        public async Task CreateAccount_Should_Return401_When_UserIsNotLoggedInd()
        {
            var createProfileDto = ProfileTestHelper.GenerateValidProfileCreateInput();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.CreateProfile(createProfileDto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        [Fact]
        public async Task CreateAccount_Should_Return403_When_UserTriesToCreateAProfileForAnotherAccount()
        {
            var createProfileDto = ProfileTestHelper.GenerateValidProfileCreateInput();
            createProfileDto.AccountId = Guid.NewGuid();

            var result = await _sut.CreateProfile(createProfileDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        // TODO: Find proper way to test for upload IFormFile for integration-tests. Otherwise test i manually


        // GetProfileById

        [Fact]
        public async Task GetProfileById_Should_Return200_When_ProfileWasFetched()
        {
            var profile = _dbContext.Profiles.First();

            var result = await _sut.GetProfileById(profile.ProfileId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProfileById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var profile = _dbContext.Profiles.First();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.GetProfileById(profile.ProfileId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        [Fact]
        public async Task GetProfileById_Should_Return404_When_NoProfileWasFound()
        {
            var result = await _sut.GetProfileById(Guid.NewGuid());

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        // UpdateProfile

        [Fact]
        public async Task UpdateProfile_Should_Return200_When_ProfileWasUpdated()
        {
            // Arrange
            var profile = _dbContext.Profiles.First();
            var updateProfile = ProfileTestHelper.GenerateValidProfileUpdateInput();        // first account and profile are supposed to be attached
            updateProfile.ProfileId = profile.ProfileId;                                    // ensures that 'updateProfile' updates existing profile

            // Act
            var result = await _sut.UpdateProfile(updateProfile);

            // Assert            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProfile_Should_Return401_When_UserIsNotLoggedIn()
        {
            // Arrange
            var profile = _dbContext.Profiles.First();
            var updateProfile = ProfileTestHelper.GenerateValidProfileUpdateInput();        // first account and profile are supposed to be attached
            updateProfile.ProfileId = profile.ProfileId;                                    // ensures that 'updateProfile' updates existing profile
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            // Act
            var result = await _sut.UpdateProfile(updateProfile);

            // Assert            
            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProfile_Should_Return403_When_UserTriesToUpdateProfileWithoutAuthorization()
        {
            // Arrange
            var profile = _dbContext.Profiles.First();
            var updateProfile = ProfileTestHelper.GenerateValidProfileUpdateInput();        // first account and profile are supposed to be attached
            updateProfile.ProfileId = ProfileTestHelper.GetValidProfileId2();               // profileId is not supposed to be associated to logged in account

            // Act
            var result = await _sut.UpdateProfile(updateProfile);

            // Assert            
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        // DeleteProfileById

        [Fact]
        public async Task DeleteProfileById_Should_Return204_When_ProfileWasDeletedFromLoggedInUser()
        {
            var profile = _dbContext.Profiles.First();

            var result = await _sut.DeleteProfileById(profile.ProfileId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProfileById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var profile = _dbContext.Profiles.First();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.DeleteProfileById(profile.ProfileId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProfileById_Should_Return403_When_UserTriesToDeleteProfileItDoesNotOwn()
        {
            var result = await _sut.DeleteProfileById(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        // SearchProfiles

        // TODO: Find proper way to test if search-functionality works
        [Fact]
        public async Task SearchProfiles_Should_Return200_When_ProfilesWereQueried()
        {
            var query = ProfileTestHelper.GenerateValidSearchQueryDTO();

            var result = await _sut.SearchQuery(query);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task SearchProfiles_Should_Return401_When_UserIsNotLoggedIN()
        {
            var query = ProfileTestHelper.GenerateValidSearchQueryDTO();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.SearchQuery(query);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        // AddToFavorites

        [Fact]
        public async Task AddToFavorites_Should_Return200_When_ProfileHasBeenAddedToLoggedInAccountFavorites()
        {
            // Act
            var result = await _sut.AddToFavorites(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddToFavorites_Should_Return401_When_UserIsNotLoggedIn()
        {
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            // Act
            var result = await _sut.AddToFavorites(Guid.NewGuid());

            // Assert
            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        [Fact]
        public async Task AddToFavorites_Should_Return409_When_UserTriesToAddOwnProfileToFavorites()
        {
            var profile = await _dbContext.Profiles.FirstAsync();  // Logged in Account supposed to contain this profile

            // Act
            var result = await _sut.AddToFavorites(profile.ProfileId);

            // Assert
            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }

        [Fact]
        public async Task AddToFavorites_Should_Return409_When_UserTriesToAddProfileToFavoritesThatAlreadyExist()
        {
            // Arrange
            var existingFavorite = ProfileTestHelper.GetValidProfileId1();      // logged in account supposed to contain this id in favorites

            // Act
            var result = await _sut.AddToFavorites(existingFavorite);

            // Assert
            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }


        // RemoveToFavorites

        [Fact]
        public async Task RemoveToFavorites_Should_Return204_When_ProfileHasBeenRemovedFromLoggedInAccountFavorites()
        {
            // Arrange
            var profileId = ProfileTestHelper.GetValidProfileId1();       // supposed to exist in logged in account favorites

            // Act
            var result = await _sut.RemoveFromFavorites(profileId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task RemoveToFavorites_Should_Return401_When_UserIsNotLoggedIn()
        {
            // Arrange
            var profileId = ProfileTestHelper.GetValidProfileId1();       // supposed to exist in logged in account favorites
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            // Act
            var result = await _sut.RemoveFromFavorites(profileId);

            // Assert
            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task RemoveToFavorites_Should_Return404_When_ProfileToBeRemovedWasNotFoundInFavoritesForLoggedInAccount()
        {
            // Arrange

            // Act
            var result = await _sut.RemoveFromFavorites(Guid.NewGuid());

            // Assert
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
