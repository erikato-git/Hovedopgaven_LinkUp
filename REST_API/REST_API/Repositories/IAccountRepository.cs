using REST_API.DTOs;
using REST_API.Models;
using REST_API.Util;

namespace REST_API.Repositories
{
    public interface IAccountRepository
    {
        // Common repository methods
        Task<Account?> GetByIdAsync(Guid id);
        Task<IEnumerable<Account>?> GetAllAsync();
        Task<Account?> AddAsync(CreateAccountDTO dto);
        Task<Account?> UpdateAsync(UpdateAccountDTO dto); 
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();

        // Custom methods
        Task<bool> doesEmailForAccountExist(String email);
        Task<Account?> GetAccountByEmail(String email);

        // Composition
        Task CreateProfile(Account account, Profile profile);
        Task DeleteProfile(Account account, Profile profile);
    }
}
