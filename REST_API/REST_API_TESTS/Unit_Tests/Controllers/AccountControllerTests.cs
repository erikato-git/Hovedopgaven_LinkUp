using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.Controllers.IHelpers;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Services.Interfaces;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using REST_API_TESTS.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

/*
 * Reference: Cummings, Neil: "Microservices ...", Section: 15
 * Naming-convention: methodName_should_returnStatusCode_when_stateUnderTest
 * Testing-targets: methods will be tested for that they return proper status-codes and objects
 * Purpose of the tests: Tests for business-logic in AccountController class, usually if-else checks for Message property in ResultDTO receiving from instance of AccountService class. The purpose is only to test how business-logic behave to response from the instance from AccountService class, why input from user is redundat in this case - it will be tested in the integrations-test
 * OBS: Authentication will be tested at integration-tests, it's not a part of the business-logic in the controller-class and is configured by dependency injection. ModelState.Valid wouldn't be tested either because validation of properties takes place on the pipeline during run-time: https://stackoverflow.com/questions/50071938/model-validation-not-working-in-unit-test
 */


namespace REST_API_TESTS.Unit_Tests.Controllers
{
    public class AccountControllerTests
    {
        /*
         * _accountService: mock to support system under test
         * _fixture: generating random values for mock
         * _sut: system under test
         */
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<IAccountControllerHelper> _accountControllerHelper;
        private readonly Fixture _fixture;
        private readonly AccountController _sut;

        public AccountControllerTests()
        {
            _fixture = new Fixture();
            _accountService = new Mock<IAccountService>();
            _accountControllerHelper = new Mock<IAccountControllerHelper>();

            _sut = new AccountController(_accountService.Object, _accountControllerHelper.Object);

        }


        // SD1: Login

        [Fact]
        public async Task Login_Should_Return200OkWithAccountWithJWT_When_ValidLoginCredentials()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var accountJWT = new { account = account, JWT = "JWT-dummy-string" };
            var resultDto = ResultDTO.SuccesResult(accountJWT, "You are succesfully logged in!");
            _accountService.Setup(service => service.Login(loginDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(accountJWT, okResult.Value);
        }


        [Fact]
        public async Task Login_Should_Return400BadRequestWithErrorMessage_When_InvalidLoginCredentials()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_Login_InvalidEmailOrPassword);
            _accountService.Setup(service => service.Login(loginDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_Login_InvalidEmailOrPassword, badResult.Value);
        }


        // SD2: CreateAccount

        [Fact]
        public async Task CreateAccount_Should_Return201OkWithAccountAndAuthentication_When_ValidCreateAccountDetails()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            var accountJWT = new { account = account, JWT = "JWT-dummy-string" };
            var resultDto = ResultDTO.SuccesResult(accountJWT, "Valid new account details");
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(accountJWT, createdResult.Value);
        }


        [Fact]
        public async Task CreateAccount_Should_Return409WithErrorMessage_When_AccountAlreadyExist()
        {
            // Arrange
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_CreateAccount_EmailForAccountAlreadyExist);
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_CreateAccount_EmailForAccountAlreadyExist, conflictResult.Value);
        }

        [Fact]
        public async Task CreateAccount_Should_Return500WithErrorMessage_When_CreateAccountFailed()
        {
            // Arrange
            var createAccountDto = AccountTestHelper.GenerateFakeInvalidCreateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_CreateAccount_CreateAccountFailed);
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.AccountSerivce_CreateAccount_CreateAccountFailed, objectResult.Value);
        }



        // SD3: UpdateAccount
        [Fact]
        public async Task UpdateAccount_Should_Return200OkWithAccount_When_AccountIsUpdated()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDto = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Updated account");
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account, okResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_Should_Return403ForbiddenWithErrorMessage_When_UserTriesToUpdateAnotherAccount()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDto = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);      // 403Forbidden
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount, objectResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_Should_Return500InternalServerErrorWithErrorMessage_When_UpdateAccountHasFailed()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = Guid.NewGuid().ToString();
            var updateAccountDto = AccountTestHelper.GenerateFakeValidUpdateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed, objectResult.Value);
        }

        // SD4: GetAccount/{id}
        [Fact]
        public async Task GetAccount_Should_Return200OkWithAccount_When_ValidId()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Account found");
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.GetAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account, okResult.Value);
        }

        [Fact]
        public async Task GetAccount_Should_Return404NotFound_When_InvalidId()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_GetAccountById_AccountNotFound);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.GetAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_AccountNotFound, notFoundResult.Value);
        }

        [Fact]
        public async Task GetAccount_Should_Return403ForbidWithErrorMessage_When_InvalidId()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_GetAccountById_YouCannotAccessAnotherAccount);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.GetAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.GetAccountById(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.AccountSerivce_GetAccountById_YouCannotAccessAnotherAccount, objectResult.Value);
        }

        // SD5: DeleteAccount/{id}
        [Fact]
        public async Task DeleteAccount_Should_Return204NoContent_When_ValidId()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var resultDto = ResultDTO.SuccesResult(true, "Account is deleted");
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.DeleteAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_Should_Return500InternalServerErrorWithErrorMessage_When_DeleteAccountFailed()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.DeleteAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed, objectResult.Value);
        }

        [Fact]
        public async Task DeleteAccount_Should_Return400BadRequestWithErrorMessage_When_InvalidId()
        {
            // Arrange
            var User = It.IsAny<ClaimsPrincipal>();
            var userAccountId = It.IsAny<Guid>().ToString();
            var id = It.IsAny<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_DeleteAccount_YouCannotDeleteAnotherAccount);
            _accountControllerHelper.Setup(service => service.ExtractUserAccountId(User)).Returns(userAccountId);
            _accountService.Setup(service => service.DeleteAccountById(id, userAccountId)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.DeleteAccountById(id);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountSerivce_DeleteAccount_YouCannotDeleteAnotherAccount, badResult.Value);
        }


    }
}
