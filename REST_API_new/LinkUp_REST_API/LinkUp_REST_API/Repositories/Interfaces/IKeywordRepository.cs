using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;

namespace LinkUp_REST_API.Repositories.Interfaces
{
    public interface IKeywordRepository
    {
        // Common repository methods
        Task<Keyword?> GetByIdAsync(Guid id);
        Task<IEnumerable<Keyword>?> GetAllAsync();
        Task<Keyword?> UpdateAsync(Keyword keyword);
        Task<bool> SaveChangesAsync();

        // Composition
        Task<Education?> CreateEducation(Keyword keyword, Education education);
        Task<bool> DeleteEducation(Keyword keyword, Education education);

    }
}
