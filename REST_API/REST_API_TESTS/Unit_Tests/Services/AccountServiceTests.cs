using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs.AccountDomain;
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

            _sut = new AccountService(_accountRepository.Object, _accountServiceHelper.Object);
        }


        // SD1: Login

        [Fact]
        public async Task Login_Should_ReturnAccountWithJWT_When_AccountExistAndPasswordsMatch()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "user@mail.com,", Password = "SecurePassword123" };
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var JWTDummy = "JWT-dummy-string";
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync(account);
            _accountServiceHelper.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account.Password)).Returns(true);
            _accountServiceHelper.Setup(service => service.GenerateJWT(account)).Returns(JWTDummy);
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
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync(account);
            _accountServiceHelper.Setup(service => service.CheckPasswordsMatch(loginDto.Password, account.Password)).Returns(false);

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
            _accountRepository.Setup(repo => repo.GetAccountByEmail(loginDto.Email)).ReturnsAsync((Account)null);

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
            _accountRepository.Setup(repo => repo.doesEmailForAccountExist(createAccountDto.Email)).ReturnsAsync(false);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.AddAsync(createAccountDto)).ReturnsAsync(account);
            var JWTDummy = "JWT-dummy-string";
            _accountServiceHelper.Setup(service => service.GenerateJWT(account)).Returns(JWTDummy);
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
            _accountRepository.Setup(repo => repo.doesEmailForAccountExist(createAccountDto.Email)).ReturnsAsync(false);
            _accountRepository.Setup(repo => repo.AddAsync(createAccountDto)).ReturnsAsync((Account)null);

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
            _accountRepository.Setup(repo => repo.doesEmailForAccountExist(createAccountDto.Email)).ReturnsAsync(true);

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
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId)).Returns(true);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.UpdateAsync(updateAccountDTO)).ReturnsAsync(account);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO);

            // Assert
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public async Task UpdateAccount_Should_ReturnErrorMessage_When_ValidUpdateAccountDetailsButUpdateFailed()
        {
            // Arrange
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId)).Returns(true);
            _accountRepository.Setup(repo => repo.UpdateAsync(updateAccountDTO)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed, result.Message);
        }

        [Fact]
        public async Task UpdateAccount_Should_ReturnErrorMessage_When_InvalidUpdateAccountDetails()
        {
            // Arrange
            var updateAccountDTO = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(updateAccountDTO.AccountId)).Returns(false);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDTO);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount, result.Message);
        }


        // SD4: GetAccount/{id}

        [Fact]
        public async Task GetAccountById_Should_ReturnAccount_When_IdIsValid()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(true);
            var account = AccountTestHelper.GenerateValidFakeAccount();
            _accountRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(account);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            Assert.Equal(account, result.Data);
        }

        [Fact]
        public async Task GetAccountById_Should_ReturnErrorMessage_When_IdIsValidButRetrieveAccountFailed()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(true);
            _accountRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Account)null);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_FailedToRetrieveAccountInternalServerError, result.Message);
        }

        [Fact]
        public async Task GetAccountById_Should_ReturnErrorMessage_When_IdIsInvalid()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(false);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_CannotRetrieveAnothersAccount, result.Message);
        }


        // SD5: DeleteAccount/{id}

        [Fact]
        public async Task DeleteAccount_Should_ReturnTrue_When_IdIsValid()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(true);
            _accountRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task DeleteAccount_Should_ReturnErrorMessage_When_IdIsValidButDeleteAccountFailed()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(true);
            _accountRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed, result.Message);
        }

        [Fact]
        public async Task DeleteAccount_Should_ReturnErrorMessage_When_IdIsInvalid()
        {
            // Arrange
            var id = It.IsAny<Guid>();
            _accountServiceHelper.Setup(service => service.CheckAccountIdMatchLoginId(id)).Returns(false);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccountById_CannotDeleteAnotherPersonsAccount, result.Message);
        }

    }
}
