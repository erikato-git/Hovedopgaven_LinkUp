using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IPortfolio
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Portfolio portfolio);
        Task SaveChangesAsync();

    }
}
