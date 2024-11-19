using Microsoft.Identity.Client;
using REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IPitchRepository
    {
        Task<IEnumerable<Pitch>?> GetPitchesByRecipientAccountIdAsync(Guid recipientAccountId);
        Task<IEnumerable<Pitch>?> GetPitchesByCreatorAsync(Guid AccountId);

        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task<Pitch> AddAsync(Pitch pitch);
        Task UpdateAsync(Pitch pitch);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
