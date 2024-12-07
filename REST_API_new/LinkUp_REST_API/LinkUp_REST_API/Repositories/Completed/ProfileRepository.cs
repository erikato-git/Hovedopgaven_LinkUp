using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Repositories.Completed
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly DataContext _dbContext;

        public ProfileRepository(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public async Task<Profile?> UpdateAsync(ProfileUpdateInput dto)
        {
            if (dto == null)
            {
                return null;
            }

            // Check if the profile exists in the database
            var existingProfile = await _dbContext.Profiles
                .Include(p => p.Account) // Include navigation properties if necessary
                .Include(p => p.Keyword)
                .FirstOrDefaultAsync(p => p.ProfileId == dto.ProfileId);

            if (existingProfile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {dto.ProfileId} was not found.");
            }

            // Update Profile properties
            if (!string.IsNullOrEmpty(dto.Profession))
            {
                existingProfile.Profession = dto.Profession;
            }

            if (!string.IsNullOrEmpty(dto.Title))
            {
                existingProfile.Title = dto.Title;
            }

            if (!string.IsNullOrEmpty(dto.AlternativeTitle))
            {
                existingProfile.AlternativeTitle = dto.AlternativeTitle;
            }

            // TODO: Media
            //if (dto.ProfilePicture != null)
            //{
            //    existingProfile.ProfilePicture = dto.ProfilePicture;
            //}

            if (!string.IsNullOrEmpty(dto.ProfileDescription))
            {
                existingProfile.ProfileDescription = dto.ProfileDescription;
            }

            // Update Keyword properties if provided
            if (existingProfile.Keyword != null)
            {
                if (!string.IsNullOrEmpty(dto.Availability))
                {
                    existingProfile.Keyword.Availability = dto.Availability;
                }

                if (dto.YearsOfExperience.HasValue)
                {
                    existingProfile.Keyword.YearsOfExperience = dto.YearsOfExperience.Value;
                }

                // Update nested Education properties
                if (existingProfile.Keyword.Education != null)
                {
                    if (!string.IsNullOrEmpty(dto.NameOfEducation))
                    {
                        existingProfile.Keyword.Education.NameOfEducation = dto.NameOfEducation;
                    }

                    if (!string.IsNullOrEmpty(dto.Institution))
                    {
                        existingProfile.Keyword.Education.Institution = dto.Institution;
                    }

                    if (dto.GraduationYear.HasValue)
                    {
                        existingProfile.Keyword.Education.GraduationYear = dto.GraduationYear.Value;
                    }
                }
            }

            // Save changes
            var saved = await SaveChangesAsync();

            if (saved)
            {
                return existingProfile;
            }

            return null;
        }





        public async Task<Profile?> GetByIdAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var profile = await _dbContext.Profiles
                .Include(k => k.Keyword)
                .ThenInclude(e => e.Education)
                // TODO: also include portfolio and audience-specification when they are implemented
                .FirstOrDefaultAsync(x => x.ProfileId == id);

            return profile;
        }

        public async Task<IEnumerable<Profile>?> GetProfilesForAccountAsync(Guid accountId)
        {
            if (string.IsNullOrEmpty(accountId.ToString()))
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            var accountExist = _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if (accountExist == null)
            {
                throw new KeyNotFoundException($"Account with ID {accountId} was not found.");
            }

            var profilesForAccount = await _dbContext.Profiles.Where(x => x.AccountId.Equals(accountId)).ToListAsync();

            return profilesForAccount;
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

            if (targetAccount.Profiles == null)
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
            if (string.IsNullOrEmpty(accountId.ToString()) || profile == null)
            {
                throw new ArgumentNullException("Invalid inputs");
            }

            var targetAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if (targetAccount == null)
            {
                return false;
            }

            // de-connect profile and account
            if (targetAccount.Profiles == null)
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
