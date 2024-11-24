using AutoFixture;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using REST_API.Data;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API_TESTS.Helpers;
using REST_API_TESTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Integration_Tests.Repositories
{
    [Collection("Shared collection")]
    public class ProfileRepositoryTests : IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private IProfileRepository _sut;
        private readonly RepositoryTestsWebFactory _factory;
        private IServiceScope _scope;
        private MssqlDbContext _dbContext;         // DbContext can easily be changed

        public ProfileRepositoryTests(RepositoryTestsWebFactory factory)
        {
            _fixture = new Fixture();
            _factory = factory;

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            _sut = new ProfileRepository(_dbContext);
        }


        // SD7: UpdateProfile

        [Fact]
        public async Task UpdateAsync_Should_ReturnProfile_When_UpdatedProfileIsValid()
        {
            // Arrange
            var profile = await _dbContext.Profiles.FirstAsync();
            profile.Profession = "Updated profession";

            // Act
            var result = await _sut.UpdateAsync(profile);

            // Assert
            Assert.Equal(profile.Profession, result?.Profession);
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnNull_When_UpdatedProfileIsInvalid()
        {
            // Arrange
            var profile = ProfileTestHelper.GenerateValidProfile();

            // Act
            var result = await _sut.UpdateAsync(profile);

            // Assert
            Assert.Equal(null, result);
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
