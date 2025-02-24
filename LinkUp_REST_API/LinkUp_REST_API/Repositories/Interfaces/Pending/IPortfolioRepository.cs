using LinkUp_REST_API.Models.Pending;

namespace LinkUp_REST_API.Repositories.Interfaces.Pending
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
