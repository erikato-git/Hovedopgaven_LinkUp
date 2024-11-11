using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs;
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


        // Login

        [Fact]
        public void Login_Should_ReturnAccountWithJWT_When_CredentialsAreValid()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Valid credentials");
            _accountRepository.Setup(repo => repo.FindAccountByEmailAndPassword(loginDto)).Returns(resultDto);
            var JWT = false;

            // Act
            var result = _sut.Login(loginDto);

            // Assert
            Assert.True(JWT);
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


        // CreateAccount

        [Fact]
        public void CreateAccount_Should_ReturnAccount_When_AccountHasBeenCreated()
        {
            // Arrange
            var createAccountDto = _fixture.Create<CreateAccountDTO>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Account has succesfully been created");
            _accountRepository.Setup(repo => repo.CreateAccount(createAccountDto)).Returns(resultDto);
            var JWT = false;

            // Act
            var result = _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.True(result.isSuccess);
            Assert.True(JWT);
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public void CreateAccount_Should_ReturnErrorMessage_When_EmailIsAlreadyTaken()
        {
            // Arrange
            var createAccountDto = _fixture.Create<CreateAccountDTO>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountRepository_CreateAccount_EmailAlreadyTaken);
            _accountRepository.Setup(repo => repo.CreateAccount(createAccountDto)).Returns(resultDto);

            // Act
            var result = _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal(ErrorMessages.AccountService_CreateAccount_409InvalidEmail, result.Message);
        }


        //UpdateAccount

        [Fact]
        public void UpdateAccount_Should_ReturnUpdatedAccount_When_AccountHasBeenUpdated()
        {
            /*
             * TODO: Make sure object in SuccesResult is the updated account
             */
            // Arrange
            var updatedAccountDto = _fixture.Create<UpdateAccountDTO>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Account has succesfully been updated");
            _accountRepository.Setup(repo => repo.UpdateAccount(updatedAccountDto)).Returns(resultDto);

            // Act
            var result = _sut.UpdateAccount(updatedAccountDto);

            // Assert
            Assert.True(result.isSuccess);
            Assert.IsType<Account>(result.Data);
        }

        [Fact]
        public void UpdateAccount_Should_ReturnErrorMessage_When_LoggedInUserTriesToUpdateAnotherAccount()
        {
            // Arrange
            var updatedAccountDto = _fixture.Create<UpdateAccountDTO>();
                // TODO: Set claim for AccountId to a specific guid

            // Act
            var result = _sut.UpdateAccount(updatedAccountDto);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal(ErrorMessages.AccountService_UpdateAccount_403CannotUpdateAnotherAccount, result.Message);
        }


        //GetAccountById

        [Fact]
        public void GetAccountById_Should_ReturnAccount_When_UserHasAuthorizationAndAccountExist()
        {
            // Arrange
            var guid = _fixture.Create<Guid>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Account is found");
            _accountRepository.Setup(repo => repo.FindAccountById(guid)).Returns(resultDto);
            var hasAuthorization = false;

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            Assert.True(result.isSuccess);
            Assert.IsType<Account>(result.Data);
            Assert.True(hasAuthorization);
        }


        [Fact]
        public void GetAccountById_Should_ReturnErrorMessage_When_LoggedInUserTriesToAccessAnotherAccount()
        {
            // Arrange
            var guid = _fixture.Create<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_GetAccountById_403UserTriesToAccessAnotherAccount);
            var hasAuthorization = false;

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal(ErrorMessages.AccountService_GetAccountById_403UserTriesToAccessAnotherAccount, result.Message);
            Assert.True(hasAuthorization);
        }


        [Fact]
        public void GetAccountById_Should_ReturnErrorMessage_When_AccountWasNotFound()
        {
            // Arrange
            var guid = _fixture.Create<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountRepository_FindAccountById_AccountWasNotFound);
            _accountRepository.Setup(repo => repo.FindAccountById(guid)).Returns(resultDto);
            var hasAuthorization = false;

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal(ErrorMessages.AccountRepository_FindAccountById_AccountWasNotFound, result.Message);
            Assert.True(hasAuthorization);
        }

    }
}
