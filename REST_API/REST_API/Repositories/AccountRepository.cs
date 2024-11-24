using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using REST_API.Data;
using REST_API.DTOs.AccountDomain;
using REST_API.Migrations;
using REST_API.Models;
using REST_API.Repositories.Interfaces;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;

namespace REST_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MssqlDbContext _dbContext;

        public AccountRepository(MssqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> GetAccountByEmailAsync(String email)
        {
            if(String.IsNullOrWhiteSpace(email))
            {  
                return null;
            }

            var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
            return account;
        }

        public async Task<bool> doesEmailForAccountExistAsync(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var found = await _dbContext.Accounts.FirstOrDefaultAsync(x =>x.Email == email);
            return found != null;
        }

        public async Task<Account?> AddAsync(Account account)
        {
            if (account != null)
            {
                var newAccount = await _dbContext.Accounts.AddAsync(account);

                var saved = await SaveChangesAsync();

                if (saved)
                {
                    return newAccount.Entity;
                }
            }

            return null;
        }

        public async Task<Account?> UpdateAsync(Account account)
        {
            if (account != null)
            {
                var found = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

                if (found != null)
                {
                    var newAccount = _dbContext.Accounts.Update(account);

                    var saved = await SaveChangesAsync();

                    if (saved)
                    {
                        return newAccount.Entity;
                    }
                }
            }

            return null;
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            var found = await _dbContext.Accounts.Include(x => x.Profiles).FirstOrDefaultAsync(x => x.AccountId == id);

            return found;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var found = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == id);

            if(found != null)
            {
                var deleted = _dbContext.Accounts.Remove(found);

                var saved = await SaveChangesAsync();

                if (saved)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<Profile?> CreateProfileAsync(Account account, Profile profile)
        {
            if(account == null || profile == null)
            {
                return null;
            }

            var targetAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

            if (targetAccount != null)
            {
                profile.AccountId = targetAccount.AccountId;        // adding profile to targetAccount
                profile.Account = targetAccount;

                if(targetAccount.Profiles != null)
                {
                    _dbContext.Profiles.Add(profile);

                    var saved = await SaveChangesAsync();

                    if (saved)
                    {
                        return profile;
                    }
                }
            }

            return null;

        }

        public async Task<bool> DeleteProfileAsync(Account account, Guid profileId)
        {
            if(account == null || String.IsNullOrEmpty(profileId.ToString()))
            {
                return false;       // TODO: research if it's more proper to throw an exception
            }

            var accountFound = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

            if (accountFound != null)
            {
                var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

                if (profileFound != null)
                {
                    var matchAccountId = profileFound.AccountId.Equals(accountFound.AccountId);

                    if (matchAccountId)
                    {
                        _dbContext.Profiles.Remove(profileFound);

                        var saved = await SaveChangesAsync();

                        if (saved)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public async Task<bool> AddSavedProfileAsync(Account account, Guid profileId)
        {
            if (account == null || String.IsNullOrEmpty(profileId.ToString()))
            {
                return false;       // TODO: research if it's more proper to throw an exception
            }

            var accountFound = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == account.AccountId);

            if (accountFound != null)
            {
                var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

                if (profileFound != null)
                {
                    var belongsToOwnAccount = accountFound.Profiles?.FirstOrDefault(x => x.ProfileId == profileId);

                    if(belongsToOwnAccount == null)
                    {
                        accountFound.SavedProfileIds?.Add(profileId);

                        var saved = await SaveChangesAsync();

                        if (saved)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Task<IEnumerable<Account>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
