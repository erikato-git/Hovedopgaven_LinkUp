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

            // profile does already have a keyword
            if( !string.IsNullOrEmpty(profileFound.KeywordId.ToString()) )
            {
                return null;
            }

            // connect keyword and profile
            keyword.ProfileId = profileFound.ProfileId;
            keyword.Profile = profileFound;
            profileFound.KeywordId = keyword.ProfileId;
            profileFound.Keyword = keyword;

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

        public async Task<bool> DeleteKeywordAsync(Guid profileId, Keyword keyword)
        {
            // null check
            if( string.IsNullOrEmpty(profileId.ToString()) || keyword == null )
            {
                throw new ArgumentNullException("Invalid arguments");
            }

            // find target profile
            var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if(profileFound == null )
            {
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // check associations
            if (profileFound.KeywordId != keyword.KeywordId)
            {
                throw new InvalidOperationException("The provided keywordId does not belong to the specified profile.");
            }

            // remove associations
            profileFound.KeywordId = null;
            profileFound.Keyword = null;

            // delete keyword
            _dbContext.Keywords.Remove(keyword);

            // save changes
            var saved = await SaveChangesAsync();

            return saved;
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
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // connect pitch and sendingProfile

            if( sendingProfile.Pitches == null )
            {
                sendingProfile.Pitches = new List<Pitch>();
            }

            pitch.Profile = sendingProfile;
            sendingProfile.Pitches.Add(pitch);

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
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // check profile is sending-profile
            if(profileFound.ProfileId != pitch.ProfileId)
            {
                throw new InvalidOperationException("The provided pitch does not belong to the specified profile.");
            }

            // check pitch exist
            var pitchFound = await _dbContext.Pitches.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if( pitchFound == null)
            {
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // detach profile and pitch
            profileFound.Pitches?.Remove(pitch);

            // delete pitch
            _dbContext.Pitches.Remove(pitchFound);

            // save changes
            var saved = await SaveChangesAsync();

            return saved;
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

            if (dto.ProfilePicture != null)
            {
                existingProfile.ProfilePicture = dto.ProfilePicture;
            }

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
                throw new KeyNotFoundException($"Account with ID {accountId} was not found.");
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
