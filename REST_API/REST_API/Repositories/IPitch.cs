using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IPitch
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task AddAsync(Pitch pitch);
        Task UpdateAsync(Pitch pitch);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
