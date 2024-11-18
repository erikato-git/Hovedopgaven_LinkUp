using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Util;

namespace REST_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public Task<Account> AddAsync(CreateAccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddSavedProfileAsync(Account account, Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task CreateProfile(Account account, Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProfile(Account account, Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> doesEmailForAccountExist(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account> UpdateAsync(UpdateAccountDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
