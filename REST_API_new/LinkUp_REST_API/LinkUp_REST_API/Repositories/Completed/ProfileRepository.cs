using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Repositories.Completed
{
    public class ProfileRepository : IProfileRepository
    {
        private DataContext _dbContext;

        public ProfileRepository(DataContext dataContext)
        {
            _dbContext = dataContext;
        }


        public async Task<Keyword?> CreateKeywordAsync(Guid profileId, Keyword keyword)
        {
            // null checks
            if( string.IsNullOrEmpty(profileId.ToString()) || keyword == null )
            {
                throw new ArgumentNullException("Invalid arguments");
            }

            // check profile exist
            var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profileId);

            if( profileFound == null )
            {
                return null;
            }

            // connect keyword to profile
            keyword.ProfileId = profileFound.ProfileId;
            keyword.Profile = profileFound;

            // create keyword
            var keywordCreated = await _dbContext.Keywords.AddAsync(keyword);

            // save changes
            var saved = await SaveChangesAsync();

            if(saved)
            {
                return keywordCreated.Entity;
            }

            return null;
        }

        public Task<bool> DeleteKeywordAsync(Guid profileId, Keyword keyword)
        {
            throw new NotImplementedException();
        }


        public async Task<Pitch?> CreatePitchAsync(Guid profileId, Pitch pitch)
        {
            // null-checks
            
            if (profileId == Guid.Empty) throw new ArgumentNullException(nameof(profileId));
            if (pitch == null) throw new ArgumentNullException(nameof(pitch));

            // attach whole profile to pitch
            
            var sendingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profileId);

            if(sendingProfile == null)
            {
                return null;
            }

            pitch.Profile = sendingProfile;

            // add pitch to dbcontext and save changes
            
            var createdPitch = _dbContext.Pitches.Add(pitch);

            var saved = await SaveChangesAsync();

            if(!saved)
            {
                return null;
            }

            return createdPitch.Entity;
        }

        public async Task<bool> DeletePitchAsync(Guid profileId, Pitch pitch)
        {
            // null checks
            if( string.IsNullOrEmpty(profileId.ToString()) || pitch == null)
            {
                throw new ArgumentNullException("Invalid arguments");
            }

            // check profile exist
            var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);  

            if( profileFound == null)
            {
                return false;
            }

            // check profile is sending-profile
            if(profileFound.ProfileId != pitch.ProfileId)
            {
                return false;
            }

            // check pitch exist
            var pitchFound = await _dbContext.Pitches.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if( pitchFound == null)
            {
                return false;
            }

            // delete pitch
            _dbContext.Pitches.Remove(pitchFound);

            // save changes
            var saved = await SaveChangesAsync();

            if(!saved)
            {
                return false;
            }

            return true;
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
                .Include(p => p.Portfolio)
                .Include(p => p.AudienceSpecification)
                .Include(p => p.Pitches)
                .FirstOrDefaultAsync(p => p.ProfileId == dto.ProfileId);

            if (existingProfile == null)
            {
                return null; // Return null if the profile does not exist
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

            if (dto.ProfilePicture != null)
            {
                existingProfile.ProfilePicture = dto.ProfilePicture;
            }

            if (!string.IsNullOrEmpty(dto.ProfileDescription))
            {
                existingProfile.ProfileDescription = dto.ProfileDescription;
            }

            // Update navigation properties if provided
            if (dto.KeywordId.HasValue)
            {
                existingProfile.KeywordId = dto.KeywordId;
            }

            if (dto.PortfolioId.HasValue)
            {
                existingProfile.PortfolioId = dto.PortfolioId;
            }

            if (dto.AudienceSpecificationId.HasValue)
            {
                existingProfile.AudienceSpecificationId = dto.AudienceSpecificationId;
            }

            if (dto.Pitches != null && dto.Pitches.Any())
            {
                existingProfile.Pitches = dto.Pitches;
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

            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == id);

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
                return null;
            }

            var profilesForAccount = await _dbContext.Profiles.Where(x => x.AccountId.Equals(accountId)).ToListAsync();

            return profilesForAccount;
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

        public Task<IEnumerable<Profile>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Portfolio?> CreatePortfolioAsync(Guid profileId, Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePortfolioAsync(Guid profileId, Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Task<AudienceSpecification?> CreateAudienceSpecificationAsync(Guid profileId, AudienceSpecification audienceSpecification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAudienceSpecificationAsync(Guid profileId, AudienceSpecification audienceSpecification)
        {
            throw new NotImplementedException();
        }

    }
}
