using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
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

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pitch>?> GetPitchesByCreatorAsync(Guid AccountId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pitch>?> GetPitchesByRecipientAccountIdAsync(Guid recipientAccountId)
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
