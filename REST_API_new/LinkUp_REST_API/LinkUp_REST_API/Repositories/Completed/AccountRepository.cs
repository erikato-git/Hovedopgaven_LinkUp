using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LinkUp_REST_API.Repositories.Completed
{
    public class AccountRepository : IAccountRepository
    {
        private DataContext _dbContext;

        public AccountRepository(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public async Task<Profile?> CreateProfileAsync(Guid accountId, Profile profile)
        {
            if (string.IsNullOrEmpty(accountId.ToString()) || profile == null)
            {
                return null;
            }

            var targetAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if (targetAccount == null)
            {
                return null;
            }

            // connect profile and account
            profile.AccountId = targetAccount.AccountId;      
            profile.Account = targetAccount;

            if(targetAccount.Profiles == null)
            {
                targetAccount.Profiles = new List<Profile>();
            }

            targetAccount.Profiles.Add(profile);

            // create profile
            _dbContext.Profiles.Add(profile);

            // save changes
            var saved = await SaveChangesAsync();

            if (saved)
            {
                return profile;
            }

            return null;
        }

        public async Task<bool> DeleteProfileAsync(Guid accountId, Profile profile)
        {
            if ( string.IsNullOrEmpty(accountId.ToString()) || profile == null )
            {
                throw new ArgumentNullException("Invalid inputs");
            }

            var targetAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if (targetAccount == null)
            {
                return false;
            }

            // de-connect profile and account
            if( targetAccount.Profiles == null)
            {
                return false;
            }

            targetAccount.Profiles.Remove(profile);

            // save changes
            var saved = await SaveChangesAsync();

            if (saved)
            {
                return true;
            }

            return false;

        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _dbContext.Accounts.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            _dbContext.Accounts.Remove(user);

            var saved = await SaveChangesAsync();

            if (saved)
            {
                return true;
            }

            return false;
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return null;
            }

            var account = await _dbContext.Accounts.Include(x => x.PersonInformation).Include(x => x.Profiles).FirstOrDefaultAsync(x => x.AccountId == id);

            return account;
        }


        public async Task<Account?> UpdateAsync(AccountUpdateInput dto)
        {
            if (dto == null)
            {
                return null;
            }

            // Check if the entity exists in the database
            var existingAccount = await _dbContext.Accounts
                .Include(a => a.PersonInformation) // Include related entities if necessary
                .FirstOrDefaultAsync(a => a.AccountId == dto.AccountId);

            if (existingAccount == null)
            {
                return null; // Return null if the account does not exist
            }

            // Update Account properties
            if (!string.IsNullOrEmpty(dto.Email))
            {
                existingAccount.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                existingAccount.Password = dto.Password;
            }

            // Update PersonInformation properties if provided
            if (!string.IsNullOrEmpty(dto.FirstName))
            {
                existingAccount.PersonInformation.FirstName = dto.FirstName;
            }

            if (!string.IsNullOrEmpty(dto.Surname))
            {
                existingAccount.PersonInformation.Surname = dto.Surname;
            }

            if (!string.IsNullOrEmpty(dto.Phone))
            {
                existingAccount.PersonInformation.Phone = dto.Phone;
            }

            if (dto.BirthDate != null && dto.BirthDate != default)
            {
                existingAccount.PersonInformation.BirthDate = dto.BirthDate.Value;
            }

            if (!string.IsNullOrEmpty(dto.Gender))
            {
                existingAccount.PersonInformation.Gender = dto.Gender;
            }

            var saved = await SaveChangesAsync();

            if (saved)
            {
                return existingAccount;
            }

            return null;
        }


        public async Task<Account?> AddAsync(Account dto)
        {
            if (dto == null)
            {
                return null;
            }

            var account = _dbContext.Accounts.Add(dto);

            var saved = await SaveChangesAsync();

            if (saved)
            {
                return account.Entity;
            }

            return null;
        }


        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var accountFound = await _dbContext.Accounts
                .Include(a => a.PersonInformation)  // Eagerly load PersonInformation
                .Include(a => a.Profiles)          // Eagerly load Profiles
                .FirstOrDefaultAsync(x => x.Email == email);

            return accountFound;
        }

        public Task<bool> AddSavedProfileAsync(Account account, Guid profileId)
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


        // TODO: Consider to place this method somewhere else, where it can be shared across multiple repository-classes
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save changes", ex);
            }
        }

    }
}
