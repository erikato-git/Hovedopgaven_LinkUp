using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;

namespace LinkUp_REST_API.Repositories.Interfaces.Pending
{
    public interface IKeywordRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Keyword keyword);
        Task SaveChangesAsync();

        // Composition
        Task CreateEducation(Keyword keyword, Education education);
        Task DeleteEducation(Keyword keyword, Education education);

    }
}
