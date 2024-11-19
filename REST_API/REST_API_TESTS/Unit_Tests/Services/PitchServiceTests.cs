using AutoFixture;
using Moq;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REST_API.Services.IHelpers;
using REST_API.Repositories;
using REST_API_TESTS.Helpers;
using REST_API.Services.Interfaces;
using REST_API.Models;
using REST_API.Util;
using REST_API.Services.Domains;

namespace REST_API_TESTS.Unit_Tests.Services
{
    public class PitchServiceTests
    {
        private readonly Mock<IPitchRepository> _pitchRepository;
        private readonly Mock<IPitchServiceHelper> _pitchServiceHelper;
        private readonly Mock<IPitchService> _pitchService;
        private readonly Fixture _fixture;
        private readonly PitchService _sut;

        public PitchServiceTests()
        {
            _fixture = new Fixture();
            _pitchRepository = new Mock<IPitchRepository>();
            _pitchServiceHelper = new Mock<IPitchServiceHelper>();
            _pitchService = new Mock<IPitchService>();

            _sut = new PitchService(_pitchRepository.Object, _pitchServiceHelper.Object);
        }


        // SD12: SendPitch

        [Fact]
        public async Task SendPitch_Should_ReturnPitch_When_PitchHasSuccesfullyBeenCreated()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _pitchServiceHelper.Setup(service => service.CheckReceiverExist(sendPitchDto.RecipientAccountId)).ReturnsAsync(true);
            _pitchServiceHelper.Setup(service => service.SendPitchDTOToPitch(sendPitchDto)).Returns(pitch);
            _pitchRepository.Setup(repo => repo.AddAsync(pitch)).ReturnsAsync(pitch);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            Assert.Equal(pitch, result.Data);
        }

        [Fact]
        public async Task SendPitch_Should_ReturnErrorMessage_When_PitchFailedToBeCreated()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _pitchServiceHelper.Setup(service => service.CheckReceiverExist(sendPitchDto.RecipientAccountId)).ReturnsAsync(true);
            _pitchServiceHelper.Setup(service => service.SendPitchDTOToPitch(sendPitchDto)).Returns(pitch);
            _pitchRepository.Setup(repo => repo.AddAsync(pitch)).ReturnsAsync((Pitch)null);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            Assert.Equal(ErrorMessages.PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task SendPitch_Should_ReturnErrorMessage_When_ReceiverForPitchDoesNotExist()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _pitchServiceHelper.Setup(service => service.CheckReceiverExist(sendPitchDto.RecipientAccountId)).ReturnsAsync(false);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            Assert.Equal(ErrorMessages.PitchService_SendPitch_ReceipientsAccountDoesNotExist, result.Message);
        }

        [Fact]
        public async Task SendPitch_Should_ReturnErrorMessage_When_AccountForLoggedInUserDoesNotExist()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            Assert.Equal(ErrorMessages.PitchService_SendPitch_AccountForLoggedInUserWasNotFound, result.Message);
        }

        [Fact]
        public async Task SendPitch_Should_ReturnErrorMessage_When_AccountWithoutAnyProfilesTriesToSendAPitch()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var account = AccountTestHelper.GenerateValidFakeAccountWithoutAnyProfiles();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            Assert.Equal(ErrorMessages.PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile, result.Message);
        }


        // SD13: IncomingPitches

        [Fact]
        public async Task IncomingPitches_Should_ReturnPitches_When_IncomingPitchesHaveBeenFetchedSuccesfully()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            var pitch1 = PitchTestHelper.GenerateValidPitch();
            var pitch2 = PitchTestHelper.GenerateValidPitch();
            IEnumerable<Pitch> pitchList = new List<Pitch> { pitch1, pitch2 };
            _pitchRepository.Setup(repo => repo.GetPitchesByRecipientAccountIdAsync(account.AccountId)).ReturnsAsync(pitchList);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            Assert.Equal(pitchList, result.Data);
        }

        [Fact]
        public async Task IncomingPitches_Should_ReturnErrorMessage_When_IncomingPitchesFailedToBeFetched()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            var pitch1 = PitchTestHelper.GenerateValidPitch();
            var pitch2 = PitchTestHelper.GenerateValidPitch();
            _pitchRepository.Setup(repo => repo.GetPitchesByRecipientAccountIdAsync(account.AccountId)).ReturnsAsync((IEnumerable<Pitch>)null);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            Assert.Equal(ErrorMessages.PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task IncomingPitches_Should_ReturnErrorMessage_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            Assert.Equal(ErrorMessages.PitchService_IncomingPitches_AccountForSignedInUserWasNotFound, result.Message);
        }


        // SD13: IncomingPitches

        [Fact]
        public async Task OutcomingPitches_Should_ReturnPitches_When_OutcomingPitchesHaveBeenFetchedSuccesfully()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            var pitch1 = PitchTestHelper.GenerateValidPitch();
            var pitch2 = PitchTestHelper.GenerateValidPitch();
            IEnumerable<Pitch> pitchList = new List<Pitch> { pitch1, pitch2 };
            _pitchRepository.Setup(repo => repo.GetPitchesByCreatorAsync(account.AccountId)).ReturnsAsync(pitchList);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            Assert.Equal(pitchList, result.Data);
        }

        [Fact]
        public async Task OutcomingPitches_Should_ReturnErrorMessages_When_OutcomingPitchesFailedToBeFetched()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync(account);
            _pitchRepository.Setup(repo => repo.GetPitchesByCreatorAsync(account.AccountId)).ReturnsAsync((IEnumerable<Pitch>)null);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            Assert.Equal(ErrorMessages.PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError, result.Message);
        }

        [Fact]
        public async Task OutcomingPitches_Should_ReturnErrorMessages_When_AccountForLoggedInUserWasNotFound()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _pitchServiceHelper.Setup(service => service.GetAccountFromLoginId()).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            Assert.Equal(ErrorMessages.PitchService_OutcomingPitches_AccountForSignedInUserWasNotFound, result.Message);
        }

    }
}
