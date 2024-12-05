using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Models;
using REST_API.Repositories.Interfaces;

namespace REST_API.Repositories
{
    public class PitchRepository : IPitchRepository
    {
        private readonly MssqlDbContext _dbContext;


        public PitchRepository(MssqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Pitch?> AddAsync(Pitch pitch)
        {
            if (pitch != null)
            {
                var senderProfileExist = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == pitch.ProfileId);

                if (senderProfileExist != null)
                {
                    var senderAccountExist = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == senderProfileExist.AccountId);

                    if (senderAccountExist != null)
                    {
                        var newAccount = await _dbContext.Pitches.AddAsync(pitch);

                        var saved = await SaveChangesAsync();

                        if (saved)
                        {
                            return newAccount.Entity;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Pitch>?> GetPitchesByRecipientAccountIdAsync(Guid recipientAccountId)
        {
            if (recipientAccountId == Guid.Empty)
            {
                return null;
            }

            var foundRecipientAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == recipientAccountId);

            if (foundRecipientAccount != null)
            {
                var pitchesWithRecipientAccountId = await _dbContext.Pitches
                                                                        .Include(x => x.Profile)
                                                                        .Where(x => x.RecipientAccountId == recipientAccountId)
                                                                        .ToListAsync();

                if (pitchesWithRecipientAccountId.Any())
                {
                    return pitchesWithRecipientAccountId;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Pitch>?> GetPitchesByCreatorAsync(Guid creatorAccountId)
        {
            if (creatorAccountId == Guid.Empty)
            {
                return null;
            }

            var creatorAccount = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == creatorAccountId);

            if (creatorAccount != null)
            {
                var pitchesWithRecipientAccountId = await _dbContext.Pitches
                                                                    .Include(x => x.Profile)
                                                                    .Where(x => x.Profile != null && x.Profile.AccountId == creatorAccountId)
                                                                    .ToListAsync();

                if (pitchesWithRecipientAccountId.Any())
                {
                    return pitchesWithRecipientAccountId;
                }
            }

            return null;


        }




        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
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

        public Task UpdateAsync(Pitch pitch)
        {
            throw new NotImplementedException();
        }
    }
}
