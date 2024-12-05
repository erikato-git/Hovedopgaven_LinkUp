using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;

namespace LinkUp_REST_API.Repositories.Interfaces
{
    public interface IKeywordRepository
    {
        // Common repository methods
        Task<Keyword?> GetByIdAsync(Guid id);
        Task<Keyword?> UpdateAsync(KeywordUpdateInput keyword);
        Task<bool> SaveChangesAsync();

        // Composition
        Task<Keyword?> CreateKeywordAsync(Guid profileId, Keyword keyword);
        Task<bool> DeleteKeywordAsync(Guid profileId, Keyword keyword);

    }
}
