using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Services;
using REST_API.Services.Helpers;
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
        private readonly Mock<IAccountServiceHelper> _accountServiceHelper;
        private readonly Fixture _fixture;
        private readonly AccountService _sut;

        public AccountServiceTests() 
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();
            _accountServiceHelper = new Mock<IAccountServiceHelper>();

            _sut = new AccountService(_accountRepository.Object);
        }


        // Login

        [Fact]
        public async Task Login_Should_ReturnAccount_When_AccountExistAndPasswordsMatch()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var loginDto = new LoginDTO { Email = "user@mail.com,", Password = "SecurePassword123" };   // make sure account.Password == loginDto.Password
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync(account);
            _accountServiceHelper.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account.Password));

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public async Task Login_Should_ReturnErrorMessage_When_AccountExistButPasswordsDontMatch()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var loginDto = _fixture.Create<LoginDTO>();
            //_accountService.Setup(service => service.Login(loginDto)).Returns(resultDto);
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync(account);
            _accountServiceHelper.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account.Password));

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_InvalidEmailOrPassword, result.Message);
        }


        [Fact]
        public async Task Login_Should_ReturnErrorMessage_When_AccountDoesntExist()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var loginDto = _fixture.Create<LoginDTO>();
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync((Account)null);
            _accountServiceHelper.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account.Password));

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_InvalidEmailOrPassword, result.Message);
        }


        // CreateAccount

        [Fact]
        public async Task CreateAccount_Should_ReturnAccountAndAuthentication_When_ValidCreateAccountDetails()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var createAccountDto = TestHelper.GenerateFakeInvalidCreateAccountDTO();
            _accountRepository.Setup(repo => repo.doesEmailForAccountExist(createAccountDto.Email)).ReturnsAsync(false);
            _accountRepository.Setup(repo => repo.AddAsync(createAccountDto)).ReturnsAsync(account);
            bool hasAuthentication = false;     // TODO

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.Equal(account, result.Data);
            Assert.True(hasAuthentication);
        }

        [Fact]
        public async Task CreateAccount_Should_ReturnErrorMessage_When_ProvidedEmailIsAlreadyTaken()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var createAccountDto = TestHelper.GenerateFakeInvalidCreateAccountDTO();
            _accountRepository.Setup(repo => repo.doesEmailForAccountExist(createAccountDto.Email)).ReturnsAsync(true);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_EmailForAccountAlreadyExist, result.Message);
        }


        // UpdateAccount
        [Fact]
        public async Task UpdateAccount_Should_ReturnAccount_When_UserIsLoggedInAndProvideValidUpdateAccountDetails()
        {
            // Arrange
            var updateAccountDto = TestHelper.GenerateFakeValidUpdateAccountDTO();
            var account = TestHelper.GenerateValidFakeAccount();
            var idClaim = updateAccountDto.AccountId;
            _accountServiceHelper.Setup(service => service.CheckIdsMatch(updateAccountDto.AccountId,idClaim)).Returns(true);   // true
            _accountRepository.Setup(repo => repo.UpdateAsync(updateAccountDto)).ReturnsAsync(account);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            Assert.Equal(account, result.Data);
        }


    }
}
