

using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Repositories.Interfaces.Completed
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
