using AutoFixture;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Moq;
using REST_API.DTOs.AccountDomain;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Domains;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace REST_API_TESTS.Unit_Tests.ServiceHelpers
{
    public class AccountServiceHelperTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Fixture _fixture;
        private readonly AccountServiceHelper _sut;

        public AccountServiceHelperTests()
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();
            _configuration = new Mock<IConfiguration>();

            _sut = new AccountServiceHelper(_accountRepository.Object, _configuration.Object);
        }

        // SD1: Login

        [Fact]
        public void CheckPasswordsMatch_Should_ReturnTrue_When_HashedPasswordOfLoginDTOMatchWithStoredPasswordForAccount()
        {
            /*
             * Make sure 'password' match with passord in Account-object in AccountTestHelper.GenerateValidFakeAccount()
             */
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var password = "Secure password";
            var hashedPassword = AccountServiceHelper.HashingPasswordWithSaltUsingSHA256(password, account.AccountId);

            // Act
            var result = _sut.CheckPasswordsMatch(password, account);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckPasswordsMatch_Should_ReturnFalse_When_HashedPasswordOfLoginDTODoesntMatchWithStoredPasswordForAccount()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var password = "Secure password1";
            var hashedPassword = AccountServiceHelper.HashingPasswordWithSaltUsingSHA256(password, account.AccountId);

            // Act
            var result = _sut.CheckPasswordsMatch(password, account);

            // Assert
            Assert.False(result);
        }


        /*
         * How to make tests containing environment-variables from appsettings.json: https://dotnetconfig.org/blog/strategies-for-testing-configuration-settings-in-unit-and-integration-tests-in-net-core/
         */
        [Fact]
        public void GenerateJWT_Should_ReturnJWT_When_AccountIsValid()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _configuration.Setup(x => x["JWT:Secret"]).Returns("HjG5^bU6&!dvS!MxrkW9-HjG5^bU6&!dvS!MxrkW9");
            _configuration.Setup(x => x["JWT:ExpirationTimeInMinutes"]).Returns("15");
            _configuration.Setup(x => x["JWT:Issuer"]).Returns("LinkUp");
            _configuration.Setup(x => x["JWT:Audience"]).Returns("Account");

            // Act
            var result = _sut.GenerateJWT(account);

            // Assert
            Assert.IsType<String>(result);      // Real testing of valid JWT will be a
        }

    }
}
