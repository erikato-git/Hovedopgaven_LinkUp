using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
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
    public class PitchControllerTests
    {
        private readonly Mock<IProfileService> _profileService;
        private readonly Fixture _fixture;
        private readonly ProfileController _sut;

        public PitchControllerTests()
        {
            _fixture = new Fixture();
            _profileService = new Mock<IProfileService>();

            _sut = new ProfileController(_profileService.Object);
        }


        // SendPitch

        [Fact]
        public async Task SendPitch_Should_Return201CreatedWithProfile_When_PitchDetailsAreValidAndReceiverExist()
        {
            // Arrange
            var sendPitchDto = PitchTestHelper.GenerateValidSendPitchDTO();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var resultDto = ResultDTO.SuccesResult(profile, "You have succesfully created a profile!");
            _profileService.Setup(service => service.CreateProfile(createProfileDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateProfile(createProfileDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(profile, createdResult.Value);
        }
    }
}
