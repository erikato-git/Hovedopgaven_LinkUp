using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Domains;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
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
        private readonly Mock<IAuthentication> _authentication;
        private readonly Fixture _fixture;
        private readonly AccountService _sut;

        public AccountServiceTests() 
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();
            _authentication = new Mock<IAuthentication>();

            _sut = new AccountService(_accountRepository.Object, _authentication.Object);
        }


        // SD1: Login

        [Fact]
        public async Task Login_Should_ReturnAccountWithJWT_When_AccountExistAndPasswordsMatch()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "user@mail.com,", Password = "SecurePassword123" };
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var JWTDummy = "JWT-dummy-string";
            _accountRepository.Setup(repo => repo.GetAccountByEmailAsync(loginDto.Email)).ReturnsAsync(account);
            _authentication.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account)).Returns(true);
            _authentication.Setup(service => service.GenerateJWT(account)).Returns(JWTDummy);
            var expected = new LoginResponseDTO { Account = account, JWT = JWTDummy };

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            var actual = Assert.IsType<LoginResponseDTO>(result.Data);
            Assert.Equal(expected.Account, actual.Account);
            Assert.Equal(expected.JWT, actual.JWT);
        }

        [Fact]
        public async Task Login_Should_ReturnErrorMessage_When_AccountExistButPasswordsDontMatch()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "user@mail.com,", Password = "SecurePassword123" };
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.GetAccountByEmailAsync(loginDto.Email)).ReturnsAsync(account);
            _authentication.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account)).Returns(false);

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_Login_InvalidEmailOrPassword, result.Message);
        }

        [Fact]
        public async Task Login_Should_ReturnErrorMessage_When_AccountDoesntExist()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "user@mail.com,", Password = "SecurePassword123" };
            _accountRepository.Setup(repo => repo.GetAccountByEmailAsync(loginDto.Email)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_Login_InvalidEmailOrPassword, result.Message);
        }


        // SD2: CreateAccount

        [Fact]
        public async Task CreateAccount_Should_ReturnAccountWithJWT_When_ValidCreateAccountDetails()
        {
            // Arrange
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            _accountRepository.Setup(repo => repo.doesEmailForAccountExistAsync(createAccountDto.Email)).ReturnsAsync(false);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.AddAsync(It.IsAny<Account>())).ReturnsAsync(account);
            var JWTDummy = "JWT-dummy-string";
            _authentication.Setup(service => service.GenerateJWT(account)).Returns(JWTDummy);
            var expected = new LoginResponseDTO { Account = account, JWT = JWTDummy };

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var actual = Assert.IsType<LoginResponseDTO>(result.Data);
            Assert.Equal(expected.Account, actual.Account);
            Assert.Equal(expected.JWT, actual.JWT);
        }

        [Fact]
        public async Task CreateAccount_Should_ReturnErrorMessage_When_ValidCreateAccountDetailsButCreateAccountDidntSucceed()
        {
            // Arrange
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            _accountRepository.Setup(repo => repo.doesEmailForAccountExistAsync(createAccountDto.Email)).ReturnsAsync(false);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.AddAsync(account)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_CreateAccount_CreateAccountFailed, result.Message);
        }

        [Fact]
        public async Task CreateAccount_Should_ReturnErrorMessage_When_EmailAlreadyTakenForCreateAccountDetails()
        {
            // Arrange
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            _accountRepository.Setup(repo => repo.doesEmailForAccountExistAsync(createAccountDto.Email)).ReturnsAsync(true);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.Equal(ErrorMessages.AccountService_CreateAccount_EmailForAccountAlreadyExist, result.Message);
        }

        // SD3: UpdateAccount

        [Fact]
        public async Task UpdateAccount_Should_ReturnAccount_When_ValidUpdateAccountDetails()
        {
            // Arrange
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            var existingAccount = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId, userAccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.GetAccountByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingAccount);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Account>())).ReturnsAsync(account);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO, userAccountId);

            // Assert
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public async Task UpdateAccount_Should_ReturnErrorMessage_When_ValidUpdateAccountDetailsButUpdateFailed()
        {
            // Arrange
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            var existingAccount = AccountTestHelper.GenerateValidFakeAccount();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId, userAccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.GetAccountByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingAccount);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Account>())).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed, result.Message);
        }

        [Fact]
        public async Task UpdateAccount_Should_ReturnErrorMessage_When_InvalidUpdateAccountDetails()
        {
            // Arrange
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId, userAccountId)).Returns(false);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount, result.Message);
        }


        // SD4: GetAccount/{id}

        [Fact]
        public async Task GetAccountById_Should_ReturnAccount_When_IdIsValid()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id,userAccountId)).Returns(true);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.GetAccountByIdAsync(id)).ReturnsAsync(account);

            // Act
            var result = await _sut.GetAccountById(id,userAccountId);

            // Assert
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public async Task GetAccountById_Should_ReturnErrorMessage_When_IdIsValidButRetrieveAccountFailed()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id, userAccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.GetAccountByIdAsync(id)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetAccountById(id, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_FailedToRetrieveAccountInternalServerError, result.Message);
        }

        [Fact]
        public async Task GetAccountById_Should_ReturnErrorMessage_When_IdIsInvalid()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id, userAccountId)).Returns(false);

            // Act
            var result = await _sut.GetAccountById(id, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_CannotRetrieveAnothersAccount, result.Message);
        }


        // SD5: DeleteAccount/{id}

        [Fact]
        public async Task DeleteAccount_Should_ReturnTrue_When_IdIsValid()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id, userAccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAccountById(id, userAccountId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task DeleteAccount_Should_ReturnErrorMessage_When_IdIsValidButDeleteAccountFailed()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id, userAccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAccountById(id, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed, result.Message);
        }

        [Fact]
        public async Task DeleteAccount_Should_ReturnErrorMessage_When_IdIsInvalid()
        {
            // Arrange
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            _authentication.Setup(service => service.CheckAccountIdMatchLoginId(id, userAccountId)).Returns(false);

            // Act
            var result = await _sut.DeleteAccountById(id, userAccountId);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccountById_CannotDeleteAnotherPersonsAccount, result.Message);
        }

    }
}
