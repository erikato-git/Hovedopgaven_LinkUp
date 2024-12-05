using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;

namespace LinkUp_REST_API.Repositories.Interfaces
{
    public interface IKeywordRepository
    {
        // Common repository methods
        Task<Keyword?> GetByIdAsync(Guid id);
        Task<IEnumerable<Keyword>?> GetAllAsync();
        Task<Keyword?> UpdateAsync(KeywordUpdateInput keyword);
        Task<bool> SaveChangesAsync();

        // Composition
        Task<Education?> CreateEducation(Guid keywordId, Education education);
        Task<bool> DeleteEducation(Guid keywordId, Education education);

    }
}
