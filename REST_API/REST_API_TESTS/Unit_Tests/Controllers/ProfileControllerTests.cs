using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Services;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.Controllers
{
    public class ProfileControllerTests
    {
        private readonly Mock<IProfileService> _profileService;
        private readonly Fixture _fixture;
        private readonly ProfileController _sut;

        public ProfileControllerTests()
        {
            _fixture = new Fixture();
            _profileService = new Mock<IProfileService>();

            _sut = new ProfileController(_profileService.Object);
        }


        // CreateProfile

        [Fact]
        public async Task CreateProfile_Should_Return201CreatedWithProfile_When_CreateProfileDetailsAreValid()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var resultDto = ResultDTO.SuccesResult(profile, "You have succesfully created a profile!");
            _profileService.Setup(service => service.CreateProfile(createProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(profile, createdResult.Value);
        }

        [Fact]
        public async Task CreateProfile_Should_Return500InternalServerErrorWithErrorMessage_When_CreateProfileFailed()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError);
            _profileService.Setup(service => service.CreateProfile(createProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError, objectResult.Value);
        }

        [Fact]
        public async Task CreateProfile_Should_Return400BadRequestWithErrorMessage_When_UserIsNotSignedIn()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfile);
            _profileService.Setup(service => service.CreateProfile(createProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfile, badResult.Value);
        }


        // UpdateProfile

        [Fact]
        public async Task UpdateProfile_Should_Return200OkWithProfile_When_UpdateProfileDetailsAreValid()
        {
            // Arrange
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var resultDto = ResultDTO.SuccesResult(profile, "Profile has succesfully been updated");
            _profileService.Setup(service => service.UpdateProfile(updateProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(profile, okResult.Value);
        }

        [Fact]
        public async Task UpdateProfile_Should_Return403ForbidWithErrorMessage_When_UpdateProfileDetailsAreInvalid()
        {
            // Arrange
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount);
            _profileService.Setup(service => service.UpdateProfile(updateProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);      // 403Forbidden
            Assert.Equal(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount, objectResult.Value);
        }


        // DeleteProfile

        [Fact]
        public async Task DeleteProfile_Should_Return204NoContent_When_ProfileIdIsValid()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var resultDto = ResultDTO.SuccesResult(true, "Profile was succesfully deleted");
            _profileService.Setup(service => service.DeleteProfile(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProfile_Should_Return500InternalServerErrorWithErrorMessage_When_DeleteProfileFailed()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError);
            _profileService.Setup(service => service.DeleteProfile(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError, objectResult.Value);
        }

        [Fact]
        public async Task DeleteProfile_Should_Return400BadRequestWithErrorMessage_When_ProfileIdIsInvalid()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_DeleteProfile_YouCannotDeleteProfileForAnotherAccount);
            _profileService.Setup(service => service.DeleteProfile(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_YouCannotDeleteProfileForAnotherAccount, badResult.Value);
        }

        // GetProfile/{id}

        [Fact]
        public async Task GetProfile_Should_Return200OkWithProfile_When_ProfileIdIsValid()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var resultDto = ResultDTO.SuccesResult(profile, "Profile was succesfully fetched");
            _profileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(profile, okResult.Value);
        }

        [Fact]
        public async Task GetProfile_Should_Return404BadRequestWithErrorMessage_When_ProfileIdIsInvalid()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId);
            _profileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId, notFoundResult.Value);
        }

        [Fact]
        public async Task GetProfile_Should_Return400BadRequestWithErrorMessage_When_LoggedInUsersAccountWasNotFound()
        {
            // Arrange
            var profileId = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_GetAccountById_AccountNotFound);
            _profileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_AccountNotFound, badResult.Value);
        }


        // SearchProfile/{query}

        [Fact]
        public async Task SearchProfile_Should_Return200OkWithProfiles_When_SearchQueryIsValid()
        {
            // Arrange
            var searchQueryDto = ProfileTestHelper.GenerateValidSearchQueryDTO();
            var profile1 = ProfileTestHelper.GenerateValidProfile();
            var profile2 = ProfileTestHelper.GenerateValidProfile();
            IEnumerable<Profile> profileList = new List<Profile> { profile1, profile2 };

            var resultDto = ResultDTO.SuccesResult(profileList, "Profiles succesfully extracted!");
            _profileService.Setup(service => service.SearchQuery(searchQueryDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SearchProfile(searchQueryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(profileList, okResult.Value);
        }

        [Fact]
        public async Task SearchProfile_Should_Return500InternalServerErrorWithErrorMessage_When_SystemFailedToQueryProfiles()
        {
            // Arrange
            var searchQueryDto = ProfileTestHelper.GenerateValidSearchQueryDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError);
            _profileService.Setup(service => service.SearchQuery(searchQueryDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SearchProfile(searchQueryDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError, objectResult.Value);
        }



    }
}
