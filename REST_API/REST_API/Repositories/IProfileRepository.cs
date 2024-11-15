using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IProfileRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Profile profile);
        Task SaveChangesAsync();

        // Composition
        Task CreateKeyword(Profile profile, Keyword keyword);
        Task DeleteKeyword(Profile profile, Keyword keyword);
        Task CreatePortfolio(Profile profile, Portfolio portfolio);
        Task DeletePortfolio(Profile profile, Portfolio portfolio);
        Task CreateAudienceSpecification(Profile profile, AudienceSpecification audienceSpecification);
        Task DeleteAudienceSpecification(Profile profile, AudienceSpecification audienceSpecification);
    }
}
