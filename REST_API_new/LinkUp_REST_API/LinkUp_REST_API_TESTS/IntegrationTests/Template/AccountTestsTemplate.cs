using AutoFixture;
using LinkUp_REST_API.Controllers;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Services;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Util;
using LinkUp_REST_API_TESTS.TestHelpers;
using LinkUp_REST_API_TESTS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

namespace LinkUp_REST_API_TESTS.IntegrationTests.Template
{
    [Collection("Shared collection")]
    public class AccountTestsTemplate : IAsyncLifetime
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

        // mocks

        //private Mock<IAccountService> _accountService;
        //private Mock<IAuthentication> _authentication;
        //private Mock<IAccountRepository> _accountRepository;
        //private Mock<DataContext> _dbContext;
        //private readonly Mock<IConfiguration> _configuration;


        public AccountTestsTemplate(ApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            // mocks

            //_accountService = new Mock<IAccountService>();
            //_authentication = new Mock<IAuthentication>();
            //_accountRepository = new Mock<IAccountRepository>();
            //_dbContext = new Mock<DataContext>();
            //_configuration = new Mock<IConfiguration>();


            // reals

            var httpContextAccessor = _factory.Services.GetRequiredService<IHttpContextAccessor>();

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
            _jwtSettings = _scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();

            _accountRepository = new AccountRepository(_dbContext);
            _authentication = new Authentication(httpContextAccessor, _jwtSettings);
            //_accountService = new AccountService(_accountRepository, _authentication);


            // Config reals / mocks

            _accountService = new AccountService(_accountRepository, _authentication);

            _sut = new AccountsController(_accountService, _authentication)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = AuthenticationTestHelper.GetClaimsPrincipal() }
                }
            };

        }

        // UpdateAccount

        //[Fact]
        //public async void UpdateAccount_Should_Return200_When_UpdateAccountInputIsValid()
        //{
        //    var updateAccountDto = AccountTestHelper.GenerateValidAccountUpdateInput();
        //    updateAccountDto.AccountId = AuthenticationTestHelper.GetValidAccountId();
        //    updateAccountDto.Email = "";

        //    var result = await _sut.UpdateAccount(updateAccountDto);

        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status200OK,okResult.StatusCode);
        //}








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
