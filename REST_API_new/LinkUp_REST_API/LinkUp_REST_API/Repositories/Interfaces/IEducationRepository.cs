using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;

namespace REST_API.Repositories.Interfaces
{
    public interface IEducationRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Education education);
        Task SaveChangesAsync();

    }
}
