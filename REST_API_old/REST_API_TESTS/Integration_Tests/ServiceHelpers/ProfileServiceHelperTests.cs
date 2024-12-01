using AutoFixture;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using REST_API.Controllers;
using REST_API.Controllers.Helpers;
using REST_API.Controllers.IHelpers;
using REST_API.Data;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
using REST_API.Services.Interfaces;
using REST_API.Util;
using REST_API_TESTS.Helpers;
using REST_API_TESTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Integration_Tests.ServiceHelpers
{
    [Collection("Shared collection")]
    public class ProfileServiceHelperTests : IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private readonly IProfileServiceHelper _sut;
        private readonly RepositoryTestsWebFactory _factory;
        private IServiceScope _scope;
        private MssqlDbContext _dbContext;         // DbContext can easily be changed
        private IPhotoAccessor _photoAccessor;

        public ProfileServiceHelperTests(RepositoryTestsWebFactory factory)
        {
            _fixture = new Fixture();
            _factory = factory;

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            //_sut = new ProfileServiceHelper(_dbContext);

        }

        // SD9: GetProfile

        [Fact]
        public async Task GetProfileFromAccount_Should_ReturnProfile_When_AccountAndProfileIdAreValid()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);
            var expectedProfile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profile.ProfileId);

            // Act
            var result = await _sut.GetProfileFromAccount(account,profile?.ProfileId);

            // Assert
            Assert.Equal(expectedProfile, result);
        }


        [Fact]
        public async Task GetProfileFromAccount_Should_ReturnNull_When_AccountAndProfileIdAreInvalid()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();

            // Act
            var result = await _sut.GetProfileFromAccount(account, Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }


        // SD10: SearchProfiles
        // TODO: Make some comprehensive test-cases for Search-profiles that proves it works as it should, and seed the test-container with many more samples (be aware of the relations)
        [Fact]
        public async Task SearchProfiles_Should_ReturnProfiles_When_ValidSearchQueryDTO()
        {
            // Arrange
            var searchQueryDto = ProfileTestHelper.GenerateValidSearchQueryDTO();

            // Act
            var result = await _sut.SearchProfiles(searchQueryDto);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<ProfileSearchResponseDTO>>(result); // Verify result is IEnumerable<Profile>
            Assert.NotNull(result); // Ensure the result is not null
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
