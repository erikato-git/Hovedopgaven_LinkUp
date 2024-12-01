using LinkUp_REST_API.Models.Pending;

namespace REST_API.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Portfolio portfolio);
        Task SaveChangesAsync();

    }
}
