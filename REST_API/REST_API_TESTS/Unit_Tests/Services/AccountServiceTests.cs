using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs;
using REST_API.Repositories;
using REST_API.Services;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Fixture _fixture;
        private readonly AccountService _sut;

        public AccountServiceTests() 
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();

            _sut = new AccountService(_accountRepository.Object);
        }

        [Fact]
        public void Login_Should_ReturnAccountWithJWT_When_CredentialsAreValid()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Valid credentials");
            _accountRepository.Setup(repo => repo.FindAccountByEmailAndPassword(loginDto)).Returns(resultDto);

            // Act
            var result = _sut.Login(loginDto);

            // Assert
            // TODO: How to return a JWT with account details
        }

        [Fact]
        public void Login_Should_ReturnGenericErrorMessage_When_EmailAndPasswordDontMatch()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountRepository_FindAccountByEmailAndPassword_EmailAndPasswordDontMatch);
            _accountRepository.Setup(repo => repo.FindAccountByEmailAndPassword(loginDto)).Returns(resultDto);

            // Act
            var result = _sut.Login(loginDto);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal(ErrorMessages.AccountService_Login_401InvalidCredentials, result.Message);
        }



    }
}
