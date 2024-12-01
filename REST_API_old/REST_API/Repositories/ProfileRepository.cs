using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Models;
using REST_API.Repositories.Interfaces;
using System.Security.Principal;

namespace REST_API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly MssqlDbContext _dbContext;
        public ProfileRepository(MssqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Profile?> UpdateAsync(Profile profile)
        {
            if (profile != null)
            {
                var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profile.ProfileId);

                if (profileFound != null)
                {
                    var updatedProfile = _dbContext.Profiles.Update(profile);

                    var saved = await SaveChangesAsync();

                    if (saved)
                    {
                        return updatedProfile.Entity;
                    }
                }
            }

            return null;
        }


        public Task CreateAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeywordAsync(Profile profile, Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public Task CreatePortfolioAsync(Profile profile, Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification)
        {
            throw new NotImplementedException();
        }

        public Task DeleteKeywordAsync(Profile profile, Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public Task DeletePortfolioAsync(Profile profile, Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
