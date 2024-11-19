using AutoFixture;
using Moq;
using REST_API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REST_API.Services.Interfaces;
using REST_API.Repositories.Interfaces;
using REST_API.DTOs.AccountDomain;
using REST_API.Repositories;
using REST_API_TESTS.Helpers;
using REST_API.Models;
using REST_API.Util;
using REST_API.DTOs.ProfileDomain;
using REST_API.Services.Domains;

namespace REST_API_TESTS.Unit_Tests.Services
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _profileRepository;
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IProfileServiceHelper> _profileServiceHelper;
        private readonly Fixture _fixture;
        private readonly ProfileService _sut;

        public ProfileServiceTests()
        {
            _fixture = new Fixture();
            _profileRepository = new Mock<IProfileRepository>();
            _profileServiceHelper = new Mock<IProfileServiceHelper>();
            _accountRepository = new Mock<IAccountRepository>();
            
            _sut = new ProfileService(_accountRepository.Object, _profileRepository.Object, _profileServiceHelper.Object);
        }

        // SD6: CreateProfile

        [Fact]
        public async Task CreateProfile_Should_ReturnProfile_When_AccountExistAndProfileWasCreated()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.CreateProfileDTOToProfile(createProfileDto)).Returns(profile);
            _accountRepository.Setup(repo => repo.CreateProfileAsync(account, profile)).ReturnsAsync(profile);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            Assert.Equal(profile, result.Data);
        }

        [Fact]
        public async Task CreateProfile_Should_ReturnErrorMessage_When_AccountExistButSystemFailedToCreateProfile()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.CreateProfileDTOToProfile(createProfileDto)).Returns(profile);
            _accountRepository.Setup(repo => repo.CreateProfileAsync(account, profile)).ReturnsAsync((Profile)null);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task CreateProfile_Should_ReturnErrorMessage_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_CouldNotFindAccountForLoggedInUser, result.Message);
        }


        // SD7: UpdateProfile

        [Fact]
        public async Task UpdateProfile_Should_ReturnProfile_When_AccountIdInUpdateProfileDTOMatchWithLoggedInAccountId()
        {
            // Arrange
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _profileServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId)).Returns(true);
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.UpdateProfileDTOToProfile(updateProfileDto)).Returns(profile);
            _profileRepository.Setup(repo => repo.UpdateAsync(profile)).ReturnsAsync(profile);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto);

            // Assert
            Assert.Equal(profile, result.Data);
        }

        [Fact]
        public async Task UpdateProfile_Should_ReturnErrorMessage_When_AccountIdAndLoggedInIdMatchButSystemFailedToUpdateProfile()
        {
            // Arrange
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _profileServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId)).Returns(true);
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.UpdateProfileDTOToProfile(updateProfileDto)).Returns(profile);
            _profileRepository.Setup(repo => repo.UpdateAsync(profile)).ReturnsAsync((Profile)null);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_UpdateProfile_FailedToUpdateProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task UpdateProfile_Should_ReturnErrorMessage_When_AccountIdAndLoggedInIdDontMatch()
        {
            // Arrange
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _profileServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId)).Returns(false);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount, result.Message);
        }


        // SD8: DeleteProfile

        [Fact]
        public async Task DeleteProfile_Should_ReturnTrue_When_ProfileHasBeenSuccesfullyDeletedFromLoggedInAccount()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.DeleteProfileAsync(account,profileId)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task DeleteProfile_Should_ReturnErrorMessage_When_ProfileFailedToBeDeletedFromLoggedInAccount()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.DeleteProfileAsync(account, profileId)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task DeleteProfile_Should_ReturnErrorMessage_When_LoggedInAccountWasNotFound()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.DeleteProfile(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToLoggedInAccountWasntFound, result.Message);
        }


        // SD9: GetProfile

        [Fact]
        public async Task GetProfile_Should_ReturnProfile_When_ProfileWasFoundInLoggedInAccount()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, profileId)).Returns(profile);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            Assert.Equal(profile, result.Data);
        }

        [Fact]
        public async Task GetProfile_Should_ReturnErrorMessage_When_ProfileWasNotFoundInLoggedInAccount()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, profileId)).Returns((Profile)null);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId, result.Message);
        }

        [Fact]
        public async Task GetProfile_Should_ReturnErrorMessage_When_LoggedInAccountWasNotFound()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetProfileById(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_GetProfile_SystemCouldntFindSignedInAccount, result.Message);
        }


        // SD10: SearchProfiles

        [Fact]
        public async Task SearchProfiles_Should_ReturnProfiles_When_QueryingProfilesSuccesfully()
        {
            // Arrange
            var query = _fixture.Create<SearchQueryDTO>();
            var profile1 = ProfileTestHelper.GenerateValidProfile();
            var profile2 = ProfileTestHelper.GenerateValidProfile();
            IEnumerable<Profile> profiles = new List<Profile> { profile1, profile2 };
            _profileServiceHelper.Setup(service => service.SearchProfiles(query)).ReturnsAsync(profiles);

            // Act
            var result = await _sut.SearchQuery(query);

            // Assert
            Assert.Equal(profiles, result.Data);
        }

        [Fact]
        public async Task SearchProfiles_Should_ReturnErrorMessage_When_QueryingProfilesFailed()
        {
            // Arrange
            var query = _fixture.Create<SearchQueryDTO>();
            var profile1 = ProfileTestHelper.GenerateValidProfile();
            var profile2 = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.SearchProfiles(query)).ReturnsAsync((List<Profile>)null);

            // Act
            var result = await _sut.SearchQuery(query);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError, result.Message);
        }


        // SD11: SaveProfile

        [Fact]
        public async Task SaveProfile_Should_ReturnTrue_When_ProfileWasSavedSuccesfully()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.AddSavedProfileAsync(account,profileId)).ReturnsAsync(true);

            // Act
            var result = await _sut.SaveProfile(profileId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task SaveProfile_Should_ReturnErrorMessage_When_ProfileFailedToBeSaved()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.AddSavedProfileAsync(account, profileId)).ReturnsAsync(false);

            // Act
            var result = await _sut.SaveProfile(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task SaveProfile_Should_ReturnErrorMessage_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            _profileServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.SaveProfile(profileId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_SystemCouldntFindSignedInAccount, result.Message);
        }

    }
}
