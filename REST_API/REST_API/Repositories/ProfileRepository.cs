using REST_API.Models;
using REST_API.Repositories.Interfaces;

namespace REST_API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
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

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Profile?> UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
