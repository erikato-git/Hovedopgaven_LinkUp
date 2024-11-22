using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.DTOs.AccountDomain;
using REST_API.Migrations;
using REST_API.Models;
using REST_API.Repositories.Interfaces;

namespace REST_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MssqlDbContext _dbContext;

        public AccountRepository(MssqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            var account = await _dbContext.Accounts.FirstAsync();

            return account;
        }

        public Task<Account?> AddAsync(CreateAccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddSavedProfileAsync(Account account, Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task<Profile?> CreateProfileAsync(Account account, Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProfileAsync(Account account, Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> doesEmailForAccountExistAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account?> UpdateAsync(UpdateAccountDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
