using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        // Common repository methods
        Task<Profile?> GetByIdAsync(Guid id);
        Task<IEnumerable<Profile>?> GetAllAsync();
        Task<Profile?> AddAsync(Profile dto);
        Task<Profile?> UpdateAsync(Profile dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();


        // Composition
        Task CreateKeywordAsync(Profile profile, Keyword keyword);
        Task DeleteKeywordAsync(Profile profile, Keyword keyword);
        Task CreatePortfolioAsync(Profile profile, Portfolio portfolio);
        Task DeletePortfolioAsync(Profile profile, Portfolio portfolio);
        Task CreateAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification);
        Task DeleteAudienceSpecificationAsync(Profile profile, AudienceSpecification audienceSpecification);
        Task CreatePitchAsync(Profile profile, Pitch pitch);
        Task DeletePitchAsync(Profile profile, Pitch pitch);    

    }
}
