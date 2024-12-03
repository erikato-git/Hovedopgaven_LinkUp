using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
using Microsoft.EntityFrameworkCore;
using REST_API.Repositories.Interfaces;

namespace LinkUp_REST_API.Repositories
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
            if( string.IsNullOrEmpty(pitchId.ToString()) )
            {
                throw new ArgumentNullException(nameof(pitchId));
            }

            var pitch = await _dbContext.Pitches.FindAsync(pitchId);

            return pitch;
        }


        public async Task<IEnumerable<Pitch>?> GetPitchesSendByAccount(Account account)
        {
            // null-checks
            if( account == null || account.Profiles == null || account.Profiles.Count() == 0 )
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

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Pitch pitch)
        {
            throw new NotImplementedException();
        }
    }
}
