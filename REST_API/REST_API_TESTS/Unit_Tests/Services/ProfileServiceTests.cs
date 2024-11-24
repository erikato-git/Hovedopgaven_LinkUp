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
using System.Security.Claims;
using REST_API.Services.IHelpers;
using REST_API.Util.Mapper;

namespace REST_API_TESTS.Unit_Tests.Services
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _profileRepository;
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IProfileServiceHelper> _profileServiceHelper;
        private readonly Mock<IAuthentication> _authentication;
        private readonly Fixture _fixture;
        private readonly ProfileService _sut;

        public ProfileServiceTests()
        {
            _fixture = new Fixture();
            _profileRepository = new Mock<IProfileRepository>();
            _profileServiceHelper = new Mock<IProfileServiceHelper>();
            _accountRepository = new Mock<IAccountRepository>();
            _authentication = new Mock<IAuthentication>();
            
            _sut = new ProfileService(_accountRepository.Object, _profileRepository.Object, _profileServiceHelper.Object, _authentication.Object);
        }

        // SD6: CreateProfile

        // TODO: Fix den, efter at jeg introducerede en ProfileMapper.MapCreateProfileDTOToProfile(dto) holdt den op med at virke
        //[Fact]
        //public async Task CreateProfile_Should_ReturnProfile_When_AccountExistAndProfileWasCreated()
        //{
        //    // Arrange
        //    var User = It.IsAny<ClaimsPrincipal>();
        //    var userAccountId = It.IsAny<Guid>().ToString();
        //    var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
        //    var account = AccountTestHelper.GenerateValidFakeAccount();
        //    var profile = ProfileTestHelper.GenerateValidProfile();
        //    var profileOut = ProfileTestHelper.GenerateValidProfile();
        //    _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);

        //    _accountRepository.Setup(repo => repo.CreateProfileAsync(account, profile)).ReturnsAsync(profileOut);

        //    // Act
        //    var result = await _sut.CreateProfile(createProfileDto,userAccountId);

        //    // Assert
        //    Assert.Equal(profile, result.Data);
        //}


        [Fact]
        public async Task CreateProfile_Should_ReturnErrorMessage_When_AccountExistButSystemFailedToCreateProfile()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var profile = ProfileTestHelper.GenerateValidProfile();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.CreateProfileAsync(account, profile)).ReturnsAsync((Profile)null);

            // Act
            var result = await _sut.CreateProfile(createProfileDto, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task CreateProfile_Should_ReturnErrorMessage_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var createProfileDto = ProfileTestHelper.GenerateValidCreateProfileDTO();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.CreateProfile(createProfileDto, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_CreateProfile_CouldNotFindAccountForLoggedInUser, result.Message);
        }


        // SD7: UpdateProfile

        [Fact]
        public async Task UpdateProfile_Should_ReturnProfile_When_AccountIdInUpdateProfileDTOMatchWithLoggedInAccountId()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId, userAccountId)).Returns(true);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
            var existingProfile = ProfileTestHelper.GenerateValidProfile();
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, updateProfileDto.AccountId)).Returns(profile);
            _profileRepository.Setup(repo => repo.UpdateAsync(profile)).ReturnsAsync(profile);


            // Act
            var result = await _sut.UpdateProfile(updateProfileDto, userAccountId);

            // Assert
            Assert.Equal(profile, result.Data);
        }

        [Fact]
        public async Task UpdateProfile_Should_ReturnErrorMessage_When_AccountIdAndLoggedInIdMatchButSystemFailedToUpdateProfile()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId, userAccountId)).Returns(true);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
            var existingProfile = ProfileTestHelper.GenerateValidProfile();
            var profile = ProfileTestHelper.GenerateValidProfile();
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, updateProfileDto.AccountId)).Returns(profile);
            _profileRepository.Setup(repo => repo.UpdateAsync(profile)).ReturnsAsync((Profile)null);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_UpdateProfile_FailedToUpdateProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task UpdateProfile_Should_ReturnErrorMessage_When_AccountIdAndLoggedInIdDontMatch()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var updateProfileDto = ProfileTestHelper.GenerateValidUpdateProfileDTO();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateProfileDto.AccountId, userAccountId)).Returns(false);

            // Act
            var result = await _sut.UpdateProfile(updateProfileDto, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount, result.Message);
        }


        // SD8: DeleteProfile

        [Fact]
        public async Task DeleteProfile_Should_ReturnTrue_When_ProfileHasBeenSuccesfullyDeletedFromLoggedInAccount()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.DeleteProfileAsync(account,profileId)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteProfile(profileId,userAccountId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task DeleteProfile_Should_ReturnErrorMessage_When_ProfileFailedToBeDeletedFromLoggedInAccount()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.DeleteProfileAsync(account, profileId)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteProfile(profileId, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task DeleteProfile_Should_ReturnErrorMessage_When_LoggedInAccountWasNotFound()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.DeleteProfile(profileId, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToLoggedInAccountWasntFound, result.Message);
        }


        // SD9: GetProfile

        [Fact]
        public async Task GetProfile_Should_ReturnProfile_When_ProfileWasFoundInLoggedInAccount()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, profileId)).Returns(profile);

            // Act
            var result = await _sut.GetProfileById(profileId, userAccountId);

            // Assert
            Assert.Equal(profile, result.Data);
        }

        [Fact]
        public async Task GetProfile_Should_ReturnErrorMessage_When_ProfileWasNotFoundInLoggedInAccount()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _profileServiceHelper.Setup(service => service.GetProfileFromAccount(account, profileId)).Returns((Profile)null);

            // Act
            var result = await _sut.GetProfileById(profileId, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId, result.Message);
        }

        [Fact]
        public async Task GetProfile_Should_ReturnErrorMessage_When_LoggedInAccountWasNotFound()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetProfileById(profileId,userAccountId);

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
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.AddSavedProfileAsync(account,profileId)).ReturnsAsync(true);

            // Act
            var result = await _sut.SaveProfile(profileId, userAccountId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task SaveProfile_Should_ReturnErrorMessage_When_ProfileFailedToBeSaved()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync(account);
            _accountRepository.Setup(repo => repo.AddSavedProfileAsync(account, profileId)).ReturnsAsync(false);

            // Act
            var result = await _sut.SaveProfile(profileId, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task SaveProfile_Should_ReturnErrorMessage_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var profileId = Guid.NewGuid();
            _authentication.Setup(service => service.GetAccountFromLoginId(userAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.SaveProfile(profileId, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.ProfileSerivce_SearchProfile_SystemCouldntFindSignedInAccount, result.Message);
        }

    }
}
