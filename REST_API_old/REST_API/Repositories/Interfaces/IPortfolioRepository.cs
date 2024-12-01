using REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Portfolio portfolio);
        Task SaveChangesAsync();

    }
}
