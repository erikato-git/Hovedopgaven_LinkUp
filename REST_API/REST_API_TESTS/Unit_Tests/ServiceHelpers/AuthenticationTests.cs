using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.ServiceHelpers
{
    public class AuthenticationTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Fixture _fixture;
        private readonly IAuthentication _sut;

        public AuthenticationTests()
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();
            _configuration = new Mock<IConfiguration>();

            _sut = new Authentication(_accountRepository.Object, _configuration.Object);
        }


        // SD3: UpdateAccount

        [Fact]
        public void CheckAccountIdMatchLoginId_Should_ReturnTrue_When_AccountIdMatchLoginAccountId()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var loginId = account.AccountId;

            // Act
            var result = _sut.CheckAccountIdMatchLoginId(account.AccountId, loginId.ToString());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckAccountIdMatchLoginId_Should_ReturnFalse_When_AccountIdAndLoginAccountIdDontMatch()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var loginId = Guid.NewGuid();

            // Act
            var result = _sut.CheckAccountIdMatchLoginId(account.AccountId, loginId.ToString());

            // Assert
            Assert.False(result);
        }




    }
}
