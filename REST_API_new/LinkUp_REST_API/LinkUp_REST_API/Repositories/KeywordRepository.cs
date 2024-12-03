using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Repositories
{
    public class KeywordRepository : IKeywordRepository
    {
        private DataContext _dbContext;

        public KeywordRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Keyword?> GetByIdAsync(Guid id)
        {
            // null checks
            if( string.IsNullOrEmpty(id.ToString()) )
            {
                throw new ArgumentNullException("id");
            }

            // get keyword
            var keyword = await _dbContext.Keywords.FirstOrDefaultAsync(x => x.KeywordId == id);    

            return keyword;
        }

        public Task<Education?> CreateEducation(Keyword keyword, Education education)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEducation(Keyword keyword, Education education)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Keyword>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Keyword?> UpdateAsync(Keyword keyword)
        {
            throw new NotImplementedException();
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
