using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IEducationRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Education education);
        Task SaveChangesAsync();

    }
}
