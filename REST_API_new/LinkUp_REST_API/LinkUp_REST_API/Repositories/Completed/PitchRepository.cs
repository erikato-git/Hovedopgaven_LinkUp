using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Repositories.Completed
{
    public class PitchRepository : IPitchRepository
    {
        private DataContext _dbContext;

        public PitchRepository(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public async Task<Pitch?> GetByIdAsync(Guid pitchId)
        {
            if (string.IsNullOrEmpty(pitchId.ToString()))
            {
                throw new ArgumentNullException(nameof(pitchId));
            }

            var pitch = await _dbContext.Pitches.FindAsync(pitchId);

            return pitch;
        }


        public async Task<IEnumerable<Pitch>?> GetPitchesSendByAccount(Account account)
        {
            // null-checks
            if (account == null || account.Profiles == null || account.Profiles.Count() == 0)
            {
                throw new ArgumentException("The profiles list cannot be null or empty.", nameof(account.Profiles));
            }

            // extract profile-ids
            var profileIds = account.Profiles.Select(p => p.ProfileId).ToList();

            // retrieve pitches where ProfileId match ProfileId (Pitch) 
            var pitches = await _dbContext.Pitches
                .Where(pitch => profileIds.Contains(pitch.ProfileId))
                .ToListAsync();

            return pitches;
        }

        public async Task<IEnumerable<Pitch>?> GetPitchesReceivedByAccount(Account account)
        {
            // null-checks
            if (account == null || account.Profiles == null || account.Profiles.Count() == 0)
            {
                throw new ArgumentException("The profiles list cannot be null or empty.", nameof(account.Profiles));
            }

            // extract profile-ids
            var profileIds = account.Profiles.Select(p => p.ProfileId).ToList();

            // retrieve pitches where ProfileId match ProfileId (Pitch) 
            var pitches = await _dbContext.Pitches
                .Where(pitch => profileIds.Contains(pitch.RecipientProfileId))
                .Include(p => p.Profile)
                .ToListAsync();

            return pitches;
        }

        public async Task<Pitch?> CreatePitchAsync(Guid profileId, Pitch pitch)
        {
            // null-checks

            if (profileId == Guid.Empty) throw new ArgumentNullException(nameof(profileId));
            if (pitch == null) throw new ArgumentNullException(nameof(pitch));

            // attach whole profile to pitch

            var sendingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profileId);

            if (sendingProfile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // connect pitch and sendingProfile

            if (sendingProfile.Pitches == null)
            {
                sendingProfile.Pitches = new List<Pitch>();
            }

            pitch.Profile = sendingProfile;
            sendingProfile.Pitches.Add(pitch);

            // add pitch to dbcontext and save changes

            var createdPitch = _dbContext.Pitches.Add(pitch);

            var saved = await SaveChangesAsync();

            if (!saved)
            {
                return null;
            }

            return createdPitch.Entity;
        }

        public async Task<bool> DeletePitchAsync(Guid profileId, Pitch pitch)
        {
            // null checks
            if (string.IsNullOrEmpty(profileId.ToString()) || pitch == null)
            {
                throw new ArgumentNullException("Invalid arguments");
            }

            // check profile exist
            var profileFound = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if (profileFound == null)
            {
                throw new KeyNotFoundException($"Profile with ID {profileId} was not found.");
            }

            // check profile is sending-profile
            if (profileFound.ProfileId != pitch.ProfileId)
            {
                throw new InvalidOperationException("The provided pitch does not belong to the specified profile.");
            }

            // check pitch exist
            var pitchFound = await _dbContext.Pitches.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if (pitchFound == null)
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
