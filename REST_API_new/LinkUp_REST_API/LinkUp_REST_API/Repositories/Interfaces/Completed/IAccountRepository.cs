using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Repositories.Interfaces.Completed
{
    public interface IAccountRepository
    {
        // Common repository methods
        Task<Account?> GetByIdAsync(Guid id);
        Task<IEnumerable<Account>?> GetAllAsync();
        Task<Account?> AddAsync(Account dto);
        Task<Account?> UpdateAsync(AccountUpdateInput dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();

        // Custom methods
        Task<bool> AddSavedProfileAsync(Account account, Guid profileId);
        Task<Account?> GetAccountByEmailAsync(string email);

        // Composition
        Task<Profile?> CreateProfileAsync(Guid accountId, Profile profile);
        Task<bool> DeleteProfileAsync(Guid accountId, Profile profile);
    }


}
