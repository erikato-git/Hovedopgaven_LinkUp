using AutoFixture;
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
        public void Login_Should_Return200Ok_When_CredentiaAreValid()
        {
            /*
             * loginDto: parameters is redundant in this case, it just has to be provided for login(LoginDTO) in AccountService and AccountController
             * ResultDTO.SuccesResult(): when calling this method the property "isSuccess" is set to true
             */
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();     
            var resultDto = ResultDTO.SuccesResult("JWT token", "Valid credentials");   
            _accountService.Setup(service => service.Login(loginDto)).Returns(resultDto);
            var JWT = false;

            // Act
            var result = _sut.Login(loginDto);

            /*
             * OkObjectResult: an OK response that contains an object
             */
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(JWT);
        }


        [Fact]
        public void Login_Should_Return400Badrequest_When_NoneConditionalChecksAreMet()
        {
            /*
             * ResultDTO.FailureResult(): takes empty strings as args so it passes all checks in AccountController until it reached BadRequest()
             */
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var resultDto = ResultDTO.FailureResult("");
            _accountService.Setup(service => service.Login(loginDto)).Returns(resultDto);

            // Act
            var result = _sut.Login(loginDto);

            /*
             * Assert.IsType<T>: checks is result match the expected T and type-cast result to T
             */
            // Assert
            var badrequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("", badrequestResult.Value);
        }

        // CreateAccount
        [Fact]
        public void CreateAccount_Should_Return201Created_When_AccountHasBeenCreated()
        {
            // Arrange
            var createAccountDto = _fixture.Create<CreateAccountDTO>();
            var resultDto = ResultDTO.SuccesResult("JWT token", "Account has succesfully been created.");
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).Returns(resultDto);
            var JWT = false;

            // Act
            var result = _sut.CreateAccount(createAccountDto);

            // Assert
            Assert.IsType<CreatedResult>(result);
            Assert.True(JWT);
        }

        [Fact]
        public void CreateAccount_Should_Return409Conflict_When_EmailIsAlreadyTaken()
        {
            // Arrange
            var createAccountDto = _fixture.Create<CreateAccountDTO>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_CreateAccount_409InvalidEmail);
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).Returns(resultDto);

            // Act
            var result = _sut.CreateAccount(createAccountDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_CreateAccount_409InvalidEmail, conflictResult.Value);
        }

        [Fact]
        public void CreateAccount_Should_Return400Badrequest_When_NoneConditionalChecksAreMet()
        {
            // Arrange
            var createAccountDto = _fixture.Create<CreateAccountDTO>();
            var resultDto = ResultDTO.FailureResult("");
            _accountService.Setup(service => service.CreateAccount(createAccountDto)).Returns(resultDto);

            // Act
            var result = _sut.CreateAccount(createAccountDto);

            // Assert
            var badrequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("", badrequestResult.Value);
        }

        // UpdateAccount

        [Fact]
        public void UpdateAccount_Should_Return200Ok_When_AccountDetailsHaveBeenUpdated()
        {
            // Arrange
            var updateAccountDTO = _fixture.Create<UpdateAccountDTO>();
            var resultDto = ResultDTO.SuccesResult(updateAccountDTO, "Account details have been updated");
            _accountService.Setup(service => service.UpdateAccount(updateAccountDTO)).Returns(resultDto);

            // Act
            var result = _sut.UpdateAccount(updateAccountDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updateAccountDTO, okResult.Value);
        }

        [Fact]
        public void UpdateAccount_Should_Return403Forbidden_When_LoggedInUserTriesToUpdateAnotherAccount()
        {
            // Arrange
            var updateAccountDTO = _fixture.Create<UpdateAccountDTO>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_UpdateAccount_403CannotUpdateAnotherAccount);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDTO)).Returns(resultDto);

            // Act
            var result = _sut.UpdateAccount(updateAccountDTO);

            /*
             * TODO: if time left, research if forbid should return any message to the user and how
             */
            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void UpdateAccount_Should_Return409Conflict_When_EmailIsAlreadyTaken()
        {
            // Arrange
            var updateAccountDTO = _fixture.Create<UpdateAccountDTO>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_UpdateAccount_409UserChangeEmailToAnotherEmailThatAlreadyExist);
            _accountService.Setup(service => service.UpdateAccount(updateAccountDTO)).Returns(resultDto);

            // Act
            var result = _sut.UpdateAccount(updateAccountDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(ErrorMessages.AccountService_UpdateAccount_409UserChangeEmailToAnotherEmailThatAlreadyExist, conflictResult.Value);
        }

        [Fact]
        public void UpdateAccount_Should_Return400Badrequest_When_NoneConditionalChecksAreMet()
        {
            // Arrange
            var updateAccountDTO = _fixture.Create<UpdateAccountDTO>();
            var resultDto = ResultDTO.FailureResult("");
            _accountService.Setup(service => service.UpdateAccount(updateAccountDTO)).Returns(resultDto);

            // Act
            var result = _sut.UpdateAccount(updateAccountDTO);

            // Assert
            var badrequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("", badrequestResult.Value);
        }

        // GetAccountById
        [Fact]
        public void GetAccountById_Should_Return200Ok_When_AccountExist()
        {
            /*
             * guid: query-parameter from URL
             * account: when GetAccountById in AccountService has succesfully received an account from AccountRepository by AccountId
             */
            // Arrange
            var guid = _fixture.Create<Guid>();
            var account = TestHelper.GenerateFakeAccount();
            var resultDto = ResultDTO.SuccesResult(account, "Account is found");
            _accountService.Setup(service => service.GetAccountById(guid)).Returns(resultDto);
            var hasAuthorization = false;

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account, okResult.Value);
            Assert.True(hasAuthorization);
        }

        [Fact]
        public void GetAccountById_Should_Return403Forbidden_When_LoggedInUserTriesToAccessAnotherAccount()
        {
            // Arrange
            var guid = _fixture.Create<Guid>();
            var resultDto = ResultDTO.FailureResult(ErrorMessages.AccountService_GetAccountById_403UserTriesToAccessAnotherAccount);
            _accountService.Setup(service => service.GetAccountById(guid)).Returns(resultDto);

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void GetAccountById_Should_Return400Badrequest_When_NoneConditionalChecksAreMet()
        {
            // Arrange
            var guid = _fixture.Create<Guid>();
            var resultDto = ResultDTO.FailureResult("");
            _accountService.Setup(service => service.GetAccountById(guid)).Returns(resultDto);

            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            var badrequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("", badrequestResult.Value);
        }

        [Fact]
        public void GetAccountById_Should_Return400BadRequest_When_ProvidedWithInvalidGuid()
        {
            // Arrange
            var guid = Guid.Empty;
            var resultDto = ResultDTO.FailureResult("");
            _accountService.Setup(service => service.GetAccountById(guid)).Returns(resultDto);


            // Act
            var result = _sut.GetAccountById(guid);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
