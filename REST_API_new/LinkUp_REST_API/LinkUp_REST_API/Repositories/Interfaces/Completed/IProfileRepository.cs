using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.DTOs.Requests.Completed;

namespace LinkUp_REST_API.Repositories.Interfaces.Completed
{
    public interface IProfileRepository
    {
        // Common repository methods
        Task<Profile?> GetByIdAsync(Guid id);
        Task<IEnumerable<Profile>?> GetAllAsync();
        Task<Profile?> UpdateAsync(ProfileUpdateInput dto);
        Task<bool> SaveChangesAsync();


        // Custom methods
        Task<IEnumerable<Profile>?> GetProfilesForAccountAsync(Guid accountId);


        // Composition
        Task<Keyword?> CreateKeywordAsync(Guid profileId, Keyword keyword);
        Task<bool> DeleteKeywordAsync(Guid profileId, Keyword keyword);
        Task<Portfolio?> CreatePortfolioAsync(Guid profileId, Portfolio portfolio);
        Task<bool> DeletePortfolioAsync(Guid profileId, Portfolio portfolio);
        Task<AudienceSpecification?> CreateAudienceSpecificationAsync(Guid profileId, AudienceSpecification audienceSpecification);
        Task<bool> DeleteAudienceSpecificationAsync(Guid profileId, AudienceSpecification audienceSpecification);
        Task<Pitch?> CreatePitchAsync(Guid profileId, Pitch pitch);
        Task<bool> DeletePitchAsync(Guid profileId, Pitch pitch);

    }
}
