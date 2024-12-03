

using LinkUp_REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IPitchRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task<bool> SaveChangesAsync();

        // Custom methods
        Task<IEnumerable<Pitch>?> GetPitchesByRecipientAccountIdAsync(Guid recipientAccountId);
        Task<IEnumerable<Pitch>?> GetPitchesByCreatorAsync(Guid AccountId);

    }
}
