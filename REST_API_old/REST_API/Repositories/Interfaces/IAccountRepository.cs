using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Util;

namespace REST_API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        // Common repository methods
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<Account>?> GetAllAsync();
        Task<Account?> AddAsync(Account dto);
        Task<Account?> UpdateAsync(Account dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();
        Task<bool> AddSavedProfileAsync(Account account, Guid profileId);

        // Custom methods
        Task<bool> doesEmailForAccountExistAsync(string email);
        Task<Account?> GetAccountByEmailAsync(string email);

        // Composition
        Task<Profile?> CreateProfileAsync(Account account, Profile profile);
        Task<bool> DeleteProfileAsync(Account account, Guid profileId);
    }

}
