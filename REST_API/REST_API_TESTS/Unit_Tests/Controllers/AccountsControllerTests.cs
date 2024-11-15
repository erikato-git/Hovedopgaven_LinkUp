using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST_API.Controllers;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Services;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AccountsControllerTests
    {
        /*
         * _accountService: mock to support system under test
         * _fixture: generating random values for mock
         * _sut: system under test
         */
        private readonly Mock<IAccountService> _accountService;
        private readonly Fixture _fixture;
        private readonly AccountController _sut;

        public AccountsControllerTests()
        {
            _fixture = new Fixture();
            _accountService = new Mock<IAccountService>();

            _sut = new AccountController(_accountService.Object);
        }


        // Login

        [Fact]
        public async Task Login_Should_Return200OkWithAccountWithJWT_When_ValidLoginCredentials()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var account = TestHelper.GenerateValidFakeAccount();
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
            var account = TestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_InvalidEmailOrPassword);
            _accountService.Setup(service => service.Login(loginDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.Login(loginDto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_InvalidEmailOrPassword, badResult.Value);
        }


        // CreateAccount

        [Fact]
        public async Task CreateAccount_Should_Return201OkWithAccountAndAuthentication_When_ValidCreateAccountDetails()
        {
            // Arrange
            var account = TestHelper.GenerateValidFakeAccount();
            var createAccountDto = TestHelper.GenerateFakeInvalidCreateAccountDTO();
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
            var createAccountDto = TestHelper.GenerateFakeInvalidCreateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_EmailForAccountAlreadyExist);
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_EmailForAccountAlreadyExist, conflictResult.Value);
        }

        [Fact]
        public async Task CreateAccount_Should_Return500WithErrorMessage_When_CreateAccountFailed()
        {
            // Arrange
            var createAccountDto = TestHelper.GenerateFakeInvalidCreateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_EmailForAccountAlreadyExist);
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.CreateAccount(createAccountDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_EmailForAccountAlreadyExist, conflictResult.Value);
        }



        // UpdateAccount
        [Fact]
        public async Task UpdateAccount_Should_Return200OkWithAccount_When_ValidUpdateAccountDetails()
        {
            // Arrange
            var updateAccountDto = TestHelper.GenerateFakeValidUpdateAccountDTO();
            var account = TestHelper.GenerateValidFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Updated account");
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto)).ReturnsAsync(resultDto);
            bool hasAuthentication = false;     // TODO

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account, okResult.Value);
            Assert.True(hasAuthentication);
        }


        [Fact]
        public async Task UpdateAccount_Should_Return403ForbiddenWithErrorMessage_When_UserTriesToUpdateAnotherAccount()
        {
            // Arrange
            var updateAccountDto = TestHelper.GenerateFakeValidUpdateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_YouCannotUpdateAnotherPersonsAccount);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto)).ReturnsAsync(resultDto);
            bool hasAuthentication = false;     // TODO

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);      // 403Forbidden
            Assert.Equal(ErrorMessages.AccountSerivce_YouCannotUpdateAnotherPersonsAccount, objectResult.Value);
            Assert.True(hasAuthentication);
        }


        [Fact]
        public async Task UpdateAccount_Should_Return401UnAuthorizedWithErrorMessage_When_InvalidUpdateAccountDetails()
        {
            // Arrange
            var updateAccountDto = TestHelper.GenerateFakeValidUpdateAccountDTO();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountSerivce_YouMustBeSignedInBeforeYouCanUpdateYourAccount);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _sut.UpdateAccount(updateAccountDto);

            // Assert
            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountSerivce_YouMustBeSignedInBeforeYouCanUpdateYourAccount, unauthResult.Value);
        }
    }
}
