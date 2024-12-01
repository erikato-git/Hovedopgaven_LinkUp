using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.Controllers.Helpers;
using REST_API.Controllers.IHelpers;
using REST_API.Models;
using REST_API.Services.Interfaces;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.Controllers
{
    public class PitchControllerTests
    {
        private readonly Mock<IPitchService> _pitchService;
        private readonly Mock<IPitchControllerHelper> _pitchControllerHelper;
        private readonly Fixture _fixture;
        private readonly PitchController _sut;

        public PitchControllerTests()
        {
            _fixture = new Fixture();
            _pitchService = new Mock<IPitchService>();
            _pitchControllerHelper = new Mock<IPitchControllerHelper>();

            _sut = new PitchController(_pitchService.Object, _pitchControllerHelper.Object);
        }


        // SD12: SendPitch

        [Fact]
        public async Task SendPitch_Should_Return201CreatedWithProfile_When_PitchDetailsAreValidAndReceiverExist()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var resultDto = ResultDTO.SuccesResult(pitch, "You have succesfully created a profile!");
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.SendPitch(sendPitchDto,userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(pitch, createdResult.Value);
        }

        [Fact]
        public async Task SendPitch_Should_Return500InternalServerErrorWithErrorMessage_When_SystemFailedToCreate()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.SendPitch(sendPitchDto,userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError, objectResult.Value);
        }

        [Fact]
        public async Task SendPitch_Should_Return404BadRequestWithErrorMessage_When_ReceiverDoesNotExist()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_SendPitch_ReceipientsAccountDoesNotExist);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.SendPitch(sendPitchDto, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(ErrorMessages.PitchService_SendPitch_ReceipientsAccountDoesNotExist, notFoundResult.Value);
        }


        [Fact]
        public async Task SendPitch_Should_Return409ConflictWithErrorMessage_When_LoggedInUserDoesNotHaveAnyProfiles()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.SendPitch(sendPitchDto, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.SendPitch(sendPitchDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile, conflictResult.Value);
        }



        // SD13: IncomingPitches

        [Fact]
        public async Task IncomingPitches_Should_Return200OkWithPitches_When_UserIsLoggedInAndPitchesAreFetchedSuccesfully()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var pitch1 = PitchTestHelper.GenerateValidPitch();
            var pitch2 = PitchTestHelper.GenerateValidPitch();
            IEnumerable<Pitch> pitchList = new List<Pitch> { pitch1, pitch2 };
            var resultDto = ResultDTO.SuccesResult(pitchList, "Pitches have succesfully been fetched!");
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetIncomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pitchList, okResult.Value);
        }


        [Fact]
        public async Task IncomingPitches_Should_Return500InternalServerErrorWithErrorMessage_When_UserIsLoggedInButPitchesFailedToBeFetched()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetIncomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError, objectResult.Value);
        }


        [Fact]
        public async Task IncomingPitches_Should_Return400NotFoundWithErrorMessage_When_AccountForSignedInUserWasNotFound()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_IncomingPitches_AccountForSignedInUserWasNotFound);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetIncomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetIncomingPitches();

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.PitchService_IncomingPitches_AccountForSignedInUserWasNotFound, badResult.Value);
        }


        // SD14: OutcomingPitches

        [Fact]
        public async Task OutcomingPitches_Should_Return200OkWithPitches_When_UserIsLoggedInAndPitchesAreFetchedSuccesfully()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var pitch1 = PitchTestHelper.GenerateValidPitch();
            var pitch2 = PitchTestHelper.GenerateValidPitch();
            IEnumerable<Pitch> pitchList = new List<Pitch> { pitch1, pitch2 };
            var resultDto = ResultDTO.SuccesResult(pitchList, "Pitches have succesfully been fetched!");
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetOutcomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pitchList, okResult.Value);
        }

        [Fact]
        public async Task OutcomingPitches_Should_Return500InternalServerErrorWithErrorMessage_When_UserIsLoggedInButPitchesFailedToBeFetched()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetOutcomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError, objectResult.Value);
        }

        [Fact]
        public async Task OutcomingPitches_Should_Return400NotFoundWithErrorMessage_When_AccountForSignedInUserWasNotFound()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.PitchService_OutcomingPitches_AccountForSignedInUserWasNotFound);
            _pitchControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _pitchService.Setup(service => service.GetOutcomingPitches(userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetOutcomingPitches();

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.PitchService_OutcomingPitches_AccountForSignedInUserWasNotFound, badResult.Value);
        }

    }
}
