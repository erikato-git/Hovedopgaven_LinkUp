using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using REST_API.Data;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services;
using REST_API.Services.Helpers;
using REST_API_TESTS.Helpers;
using REST_API_TESTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Integration_Tests.Repositories
{
    /*
     * IClassFixture: enables the test-class to get access to all the resources in the ApplicationWebFactory-class and can be shared accross multiple test-classes
     * IAsyncLifeTime: implements methods InitializeAsync() and DisposeAsync() that are executed before and after each test
     * Reference: Cummings, Neil: "Build a Microservice app with .NET and NextJS from scratch", lecture: 194, Udemy
     */
    public class AccountRepositoryTests : IClassFixture<RepositoryTestsWebFactory>, IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private IAccountRepository _sut;
        private readonly RepositoryTestsWebFactory _factory;
        private IServiceScope _scope;

        public AccountRepositoryTests(RepositoryTestsWebFactory factory)
        {
            _fixture = new Fixture();
            _factory = factory;

            _scope = _factory.Services.CreateScope(); 
            var dbContext = _scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            _sut = new AccountRepository(dbContext); 
        }

        // SD6: CreateProfile

        [Fact]
        public async Task GetAccountByProfile_Should_ReturnAccount_When_EmailIsValidAndFound()
        {
            // Arrange
            var email = "hej";

            // Act
            var result = await _sut.GetAccountByEmailAsync(email);

            // Assert
            Assert.Null(result);
        }










        // Re-initialize test-database
        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            DbInitializerForTests.ReinitDbForTests(db);
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
