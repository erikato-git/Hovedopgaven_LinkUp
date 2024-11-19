using REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task<Profile?> UpdateAsync(Profile profile);
        Task SaveChangesAsync();

        // Composition
        Task CreateKeywordAsync(Profile profile, Keyword keyword);
        Task DeleteKeywordAsync(Profile profile, Keyword keyword);
        Task CreatePortfolioAsync(Profile profile, Portfolio portfolio);
        Task DeletePortfolioAsync(Profile profile, Portfolio portfolio);
        Task CreateAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification);
        Task DeleteAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification);
    }
}
