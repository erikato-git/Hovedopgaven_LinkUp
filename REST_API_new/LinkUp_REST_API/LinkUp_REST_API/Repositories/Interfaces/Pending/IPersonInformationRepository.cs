
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Repositories.Interfaces.Pending
{
    public interface IPersonInformationRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task AddAsync(PersonInformation personinformation);
        Task UpdateAsync(PersonInformation personinformation);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();

    }
}
