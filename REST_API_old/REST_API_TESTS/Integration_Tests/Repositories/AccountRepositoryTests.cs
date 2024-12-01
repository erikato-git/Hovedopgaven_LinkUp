using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using REST_API.Data;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services;
using REST_API.Services.Domains;
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
    [Collection("Shared collection")]
    public class AccountRepositoryTests : IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private IAccountRepository _sut;
        private readonly RepositoryTestsWebFactory _factory;
        private IServiceScope _scope;
        private MssqlDbContext _dbContext;         // DbContext can easily be changed


        public AccountRepositoryTests(RepositoryTestsWebFactory factory)
        {
            _fixture = new Fixture();
            _factory = factory;

            _scope = _factory.Services.CreateScope(); 
            _dbContext = _scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            _sut = new AccountRepository(_dbContext);

        }

        // SD1: GetAccountByEmail

        [Fact]
        public async Task GetAccountByEmail_Should_ReturnAccount_When_EmailIsValidAndFound()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.GetAccountByEmailAsync(account.Email);

            // Assert
            Assert.Equal(account.Email, result?.Email);
        }


        [Fact]
        public async Task GetAccountByEmail_Should_ReturnNull_When_EmailIsInvalidAndNotFound()
        {
            // Arrange

            // Act
            var result = await _sut.GetAccountByEmailAsync("Invalid@Email");

            // Assert
            Assert.Null(result);
        }

        // SD2: DoesEmailForAccountExist

        [Fact]
        public async Task DoesEmailForAccountExist_Should_ReturnTrue_When_EmailAlreadyExist()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.doesEmailForAccountExistAsync(account.Email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesEmailForAccountExist_Should_ReturnFalse_When_EmailDoesNotExist()
        {
            // Arrange

            // Act
            var result = await _sut.doesEmailForAccountExistAsync("Invalid@Email");

            // Assert
            Assert.False(result);
        }

        // SD2: AddAsync

        [Fact]
        public async Task AddAsync_Should_ReturnAccount_When_NewAccountIsValid()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();
            var expectedLength = _dbContext.Accounts.Count() + 1;

            // Act
            var result = await _sut.AddAsync(account);

            // Assert
            var actualLength = _dbContext.Accounts.Count();
            Assert.Equal(account, result);
            Assert.Equal(expectedLength, actualLength);
        }


        [Fact]
        public async Task AddAsync_Should_ReturnNull_When_NewAccountIsInvalid()
        {
            // Arrange

            // Act
            var result = await _sut.AddAsync((Account)null);

            // Assert
            Assert.Null(result);
        }

        // SD3: UpdateAsync

        [Fact]
        public async Task UpdateAsync_Should_ReturnAccount_When_UpdateAccountIsValid()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            account.Email = "ChangeMail@mail.com";

            // Act
            var result = await _sut.UpdateAsync(account);

            // Assert
            Assert.Equal(account, result);
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnAccount_When_UpdateAccountIsInvalid()
        {
            // Arrange
            var account = AccountTestHelper.GenerateValidFakeAccount();

            // Act
            var result = await _sut.UpdateAsync(account);

            // Assert
            Assert.Null(result);
        }

        // SD4: GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_Should_ReturnAccount_When_IdIsValid()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.GetAccountByIdAsync(account.AccountId);

            // Assert
            Assert.Equal(account, result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnAccount_When_IdIsInvalid()
        {
            // Arrange

            // Act
            var result = await _sut.GetAccountByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null((Account)result);
        }

        // SD5: DeleteAsync

        [Fact]
        public async Task DeleteAsync_Should_ReturnTrue_When_AccountHasBeenRemoved()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var expectedLength = _dbContext.Accounts.Count() - 1;

            // Act
            var result = await _sut.DeleteAsync(account.AccountId);

            // Assert
            var actualLength = _dbContext.Accounts.Count();
            Assert.Equal(expectedLength, actualLength);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnFalse_When_AccountIdIsInvalid()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        // SD6: CreateProfileAsync

        [Fact]
        public async Task CreateProfileAsync_Should_ReturnProfile_When_ProfileHasBeenAddedToAccountThatExist()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var profile = ProfileTestHelper.GenerateValidProfile();
            var expectedLength = account.Profiles?.Count() + 1;

            // Act
            var result = await _sut.CreateProfileAsync(account, profile);

            // Assert
            var actualLength = account.Profiles?.Count();
            Assert.True(account.Profiles?.Contains(result));
            Assert.Equal(expectedLength, actualLength);
        }

        [Fact]
        public async Task CreateProfileAsync_Should_ReturnNull_When_ProfileOrAccountIsNull()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            Profile? result = await _sut.CreateProfileAsync(account, (Profile)null);

            // Assert
            Assert.Null(result);
        }


        // SD8: DeleteProfile

        [Fact]
        public async Task DeleteProfile_Should_ReturnTrue_When_ProfileHasBeenDeletedFromExistingAccount()
        {
            /*
             * profile and account are arranged to match in DbInitializer
             */
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

            // Act
            var result = await _sut.DeleteProfileAsync(account,profile.ProfileId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProfile_Should_ReturnTrue_When_ProfileFromExistingAccountIsInvalid()
        {
            // Arrange
            var profile = ProfileTestHelper.GenerateValidProfile();
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.DeleteProfileAsync(account, profile.ProfileId);

            // Assert
            Assert.False(result);
        }

        // SD11: AddSavedProfile

        [Fact]
        public async Task AddSavedProfile_Should_ReturnTrue_When_ProfileHasBeenSaveForLaterVistForAnAccount()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.AccountId != account.AccountId);

            // Act
            var result = await _sut.AddSavedProfileAsync(account, profile.ProfileId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddSavedProfile_Should_ReturnFalse_When_TryToAddInvalidProfileIdForLaterVistToAnAccount()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.AddSavedProfileAsync(account, Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddSavedProfile_Should_ReturnFalse_When_TryToAddProfileFromOwnAccount()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

            // Act
            var result = await _sut.AddSavedProfileAsync(account, profile.ProfileId);

            // Assert
            Assert.False(result);
        }




        // Re-initialize test-database
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
