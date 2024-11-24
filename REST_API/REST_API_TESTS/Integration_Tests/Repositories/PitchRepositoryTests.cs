using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using REST_API.Data;
using REST_API.Repositories.Interfaces;
using REST_API.Repositories;
using REST_API_TESTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REST_API_TESTS.Helpers;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;

namespace REST_API_TESTS.Integration_Tests.Repositories
{
    [Collection("Shared collection")] 
    public class PitchRepositoryTests : IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private IPitchRepository _sut;
        private readonly RepositoryTestsWebFactory _factory;
        private IServiceScope _scope;
        private MssqlDbContext _dbContext;         // DbContext can easily be changed

        public PitchRepositoryTests(RepositoryTestsWebFactory factory)
        {
            _fixture = new Fixture();
            _factory = factory;

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            _sut = new PitchRepository(_dbContext);
        }

        // SD12: SendPitch

        [Fact]
        public async Task AddAscync_Should_ReturnPitch_When_NewPitchHasBeenCreated()
        {
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var pitch = PitchTestHelper.GenerateValidPitch();
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);
            pitch.ProfileId = profile.ProfileId;
            pitch.Profile = profile;

            // Act
            var result = await _sut.AddAsync(pitch);

            // Assert
            Assert.Equal(pitch, result);
        }

        [Fact]
        public async Task AddAscync_Should_ReturnNull_When_InvalidPitchHasBeenInserted()
        {
            // Arrange
            var pitch = PitchTestHelper.GenerateValidPitch();

            // Act
            var result = await _sut.AddAsync(pitch);

            // Assert
            Assert.Null(result);
        }

        // SD13: IncomingPitches

        [Fact]
        public async Task GetPitchesByRecipientAccountIdAsync_Should_ReturnPitches_When_PichesWithParticularRecipientAccountIdHaveBeenFetched()
        {
            /*
             * Pitches should be attached to first Account-element by RecipientAccountId in DbInitializerForTests
             */
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var accountId = account.AccountId;

            // Act
            var result = await _sut.GetPitchesByRecipientAccountIdAsync(accountId);

            // Assert
            var actual = Assert.IsAssignableFrom<IEnumerable<Pitch>>(result);
            var recipientAccountId = actual.First().RecipientAccountId;
            Assert.Equal(accountId, recipientAccountId);
        }

        [Fact]
        public async Task GetPitchesByRecipientAccountIdAsync_Should_ReturnNull_When_RecipientAccountIdIsInvalid()
        {
            // Arrange

            // Act
            var result = await _sut.GetPitchesByRecipientAccountIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        // SD14: OutcomingPitches

        [Fact]
        public async Task GetPitchesByCreator_Should_ReturnPitches_When_PitchesHaveProfileWithAccountIdThatMatchInput()
        {
            /*
             * Pitches containing a Profile with an AccountId that match first Account-element should be arranged in DbInitializerForTests
             */
            // Arrange
            var account = await _dbContext.Accounts.FirstAsync();
            var accountId = account.AccountId;

            // Act
            var result = await _sut.GetPitchesByCreatorAsync(accountId);

            // Assert
            var actual = Assert.IsAssignableFrom<IEnumerable<Pitch>>(result);
            var creatorAccountId = actual.First()?.Profile?.AccountId;
            Assert.Equal(accountId, creatorAccountId);
        }

        [Fact]
        public async Task GetPitchesByCreator_Should_ReturnNull_When_NoPitchesHaveProfileWithAccountIdThatMatchInput()
        {
            // Arrange

            // Act
            var result = await _sut.GetPitchesByCreatorAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
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
