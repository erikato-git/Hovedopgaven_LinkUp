using AutoFixture;
using LinkUp_REST_API.Controllers.Completed;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Repositories.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services;
using LinkUp_REST_API.Services.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;
using LinkUp_REST_API_TESTS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkUp_REST_API_TESTS.IntegrationTests.Completed
{
    [Collection("Shared collection")]
    public class AccountTests : IAsyncLifetime
    {
        private ApplicationFactory _factory;
        private Fixture _fixture;
        private IServiceScope _scope;

        // reals

        private AccountsController _sut;

        private IAccountService _accountService;
        private IAuthentication _authentication;
        private IAccountRepository _accountRepository;
        private DataContext _dbContext;
        private IOptions<JwtSettings> _jwtSettings;

        public AccountTests(ApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            // reals

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
            _jwtSettings = _scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();

            _accountRepository = new AccountRepository(_dbContext);
            _authentication = new Authentication(_jwtSettings);
            _accountService = new AccountService(_accountRepository, _authentication);


            _sut = new AccountsController(_accountService, _authentication)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = AuthenticationTestHelper.GetClaimsPrincipal() }
                }
            };

        }

        // Login
        [Fact]
        public async Task Login_Should_Return200WithJWT_When_LoginInputIsValid()
        {
            var validLoginDto = AccountTestHelper.GetValidLoginInput();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);
            var account = AccountTestHelper.GenerateValidAccount();

            var result = await _sut.Login(validLoginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            // TODO: find proper way to test for JWT is returned
        }

        [Fact]
        public async Task Login_Should_Return400_When_UserIsAlreadyLoggedInAsync()
        {
            var validLoginDto = AccountTestHelper.GetValidLoginInput();

            var result = await _sut.Login(validLoginDto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }


        [Fact]
        public async Task Login_Should_Return400_When_LoginInputPasswordIsInvalid()
        {
            var loginDto = AccountTestHelper.GetValidLoginInput();
            loginDto.Password = "Invalid Password";

            var result = await _sut.Login(loginDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }


        [Fact]
        public async Task Login_Should_Return400_When_LoginInputEmailIsInvalid()
        {
            var loginDto = AccountTestHelper.GetValidLoginInput();
            loginDto.Email = "InvalidEmail";

            var result = await _sut.Login(loginDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }


        // CreateAccount

        [Fact]
        public async Task CreateAccount_Should_Return201_When_CreateAccountInputIsValid()
        {
            var createAccountDto = AccountTestHelper.GenerateValidAccountCreateInput();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.CreateAccount(createAccountDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }


        [Fact]
        public async Task CreateAccount_Should_Return400_When_UserIsLoggedInAndTriesToCreateNewAccount()
        {
            var createAccountDto = AccountTestHelper.GenerateValidAccountCreateInput();

            var result = await _sut.CreateAccount(createAccountDto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }


        [Fact]
        public async Task CreateAccount_Should_Return409_When_UserIsUnder13YearsOld()
        {
            var createAccountDto = AccountTestHelper.GenerateValidAccountCreateInput();
            createAccountDto.BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-12));
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);


            var result = await _sut.CreateAccount(createAccountDto);

            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }

        // UpdateAccount

        [Fact]
        public async Task UpdateAccount_Should_Return200_When_UpdateAccountInputIsValid()
        {
            var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
            updateAccountDto.Password = AccountTestHelper.GetValidPassword();
            updateAccountDto.AccountId = AuthenticationTestHelper.GetValidAccountId1();
            updateAccountDto.Email = "";

            var result = await _sut.UpdateAccount(updateAccountDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAccount_Should_Return401_When_UserIsNotLoggedIn()
        {
            var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.UpdateAccount(updateAccountDto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        [Fact]
        public async Task UpdateAccount_Should_Return403_When_LoggedInUserTriesToUpdateAnotherAccount()
        {
            var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
            updateAccountDto.AccountId = Guid.NewGuid();

            var result = await _sut.UpdateAccount(updateAccountDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }


        [Fact]
        public async Task UpdateAccount_Should_Return409_When_LoggedInUserTriesToChangeEmailToAnExistingEmail()
        {
            var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
            updateAccountDto.Password = AccountTestHelper.GetValidPassword();
            updateAccountDto.Email = AccountTestHelper.GetValidEmail();     // existing mail

            var result = await _sut.UpdateAccount(updateAccountDto);

            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
        }


        [Fact]
        public async Task UpdateAccount_Should_Return404_When_TargetAccountDoesNotExist()
        {
            var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
            var newId = Guid.NewGuid();     // not existing account
            updateAccountDto.AccountId = newId;
            AuthenticationTestHelper.SetAccountIdClaimInHttpContext(_sut.ControllerContext, newId);

            var result = await _sut.UpdateAccount(updateAccountDto);

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        // GetExternalAccountById

        [Fact]
        public async Task GetExternalAccountById_Should_Return200_When_UpdateAccountInputIsValid()
        {
            var validId = AuthenticationTestHelper.GetValidAccountId2();        // AccountId exist but different from AccountId for logged-in-user

            var result = await _sut.GetExternalAccountById(validId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }


        [Fact]
        public async Task GetExternalAccountById_Should_Return401_When_UserIsNotLoggedIn()
        {
            var validId = AuthenticationTestHelper.GetValidAccountId2();        // AccountId exist but different from AccountId for logged-in-user
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);

            var result = await _sut.GetExternalAccountById(validId);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }


        [Fact]
        public async Task GetExternalAccountById_Should_Return403_When_LoggedInUserTriesToAccesOwnAccount()
        {
            var validAccountId = AuthenticationTestHelper.GetValidAccountId1();

            var result = await _sut.GetExternalAccountById(validAccountId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }


        [Fact]
        public async Task GetExternalAccountById_Should_Return404_When_UserTriesToAccesAccountThatDoesntExist()
        {
            var result = await _sut.GetExternalAccountById(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }


        // GetOwnAccount

        [Fact]
        public async Task GetOwnAccount_Should_Return200_When_LoggedInUserGetsOwnAccount()
        {
            var result = await _sut.GetOwnAccount();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetOwnAccount_Should_Return401_When_UserIsNotLoggedIn()
        {
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);
            var result = await _sut.GetOwnAccount();

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task GetOwnAccount_Should_Return404_When_LoggedInUserWasNotFound()
        {
            AuthenticationTestHelper.SetAccountIdClaimInHttpContext(_sut.ControllerContext, Guid.NewGuid());

            var result = await _sut.GetOwnAccount();

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        // DeleteOwnAccount

        [Fact]
        public async Task DeleteOwnAccount_Should_Return200_When_LoggedInUserDeletesOwnAccount()
        {
            var dto = new AccountDeleteInput
            { 
                Password = AccountTestHelper.GetValidPassword() 
            };

            var result = await _sut.DeleteOwnAccount(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOwnAccount_Should_Return401_When_UserIsNotLoggedIn()
        {
            AuthenticationTestHelper.ResetHttpContext(_sut.ControllerContext);
            var dto = new AccountDeleteInput
            {
                Password = AccountTestHelper.GetValidPassword()
            };

            var result = await _sut.DeleteOwnAccount(dto);

            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOwnAccount_Should_Return404_When_LoggedInUserWasNotFound()
        {
            AuthenticationTestHelper.SetAccountIdClaimInHttpContext(_sut.ControllerContext, Guid.NewGuid());
            
            var dto = new AccountDeleteInput
            {
                Password = AccountTestHelper.GetValidPassword()
            };

            var result = await _sut.DeleteOwnAccount(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }








        public Task DisposeAsync()
        {
            DbInitializerForTests.ReinitDbForTests(_dbContext);
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

    }

}
