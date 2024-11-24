using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Moq;
using REST_API.DTOs.AccountDomain;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Domains;
using REST_API.Services.Helpers;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.ServiceHelpers
{
    public class AccountServiceHelperTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        //private readonly Mock<IAccountServiceHelper> _accountServiceHelperMock;
        private readonly Fixture _fixture;
        private readonly AccountServiceHelper _sut;

        public AccountServiceHelperTests()
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();
            //_accountServiceHelperMock = new Mock<IAccountServiceHelper>();

            _sut = new AccountServiceHelper(_accountRepository.Object);
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
            /*
             * Make sure 'password' match with passord in Account-object in AccountTestHelper.GenerateValidFakeAccount()
             */
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var password = "Secure password1";
            var hashedPassword = AccountServiceHelper.HashingPasswordWithSaltUsingSHA256(password, account.AccountId);

            // Act
            var result = _sut.CheckPasswordsMatch(password, account);

            // Assert
            Assert.False(result);
        }

    }
}
