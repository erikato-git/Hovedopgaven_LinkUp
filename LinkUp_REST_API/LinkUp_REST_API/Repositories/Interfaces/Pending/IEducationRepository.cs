using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Repositories.Interfaces.Pending
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
