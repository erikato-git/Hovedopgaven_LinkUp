

using LinkUp_REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IPitchRepository
    {
        // Common repository methods
        Task<Pitch?> GetByIdAsync(Guid id);
        Task<bool> SaveChangesAsync();

        // Custom methods
        Task<IEnumerable<Pitch>?> GetPitchesReceivedByAccount(Account account);
        Task<IEnumerable<Pitch>?> GetPitchesSendByAccount(Account account);

    }
}
