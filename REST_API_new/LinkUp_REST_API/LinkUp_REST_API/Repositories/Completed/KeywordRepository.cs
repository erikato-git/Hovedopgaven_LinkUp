using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Repositories.Completed
{
    public class KeywordRepository : IKeywordRepository
    {
        private DataContext _dbContext;

        public KeywordRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Keyword?> UpdateAsync(KeywordUpdateInput dto)
        {
            if (dto == null)
            {
                return null;
            }

            // Check if the keyword exists in the database
            var existingKeyword = await _dbContext.Keywords
                .Include(k => k.Education)  // Include Education navigation property
                .FirstOrDefaultAsync(k => k.KeywordId == dto.KeywordId);

            if (existingKeyword == null)
            {
                return null; // Return null if the keyword does not exist
            }

            // Update Keyword properties
            if (!string.IsNullOrEmpty(dto.Availability))
            {
                existingKeyword.Availability = dto.Availability;
            }

            if (dto.YearsOfExperience.HasValue)
            {
                existingKeyword.YearsOfExperience = dto.YearsOfExperience.Value;
            }

            // Update Education properties if provided
            if (!string.IsNullOrEmpty(dto.NameOfEducation) || !string.IsNullOrEmpty(dto.Institution) || dto.GraduationYear.HasValue)
            {
                if (existingKeyword.Education == null)
                {
                    // If Education is not set, create a new one
                    existingKeyword.Education = new Education();
                }

                if (!string.IsNullOrEmpty(dto.NameOfEducation))
                {
                    existingKeyword.Education.NameOfEducation = dto.NameOfEducation;
                }

                if (!string.IsNullOrEmpty(dto.Institution))
                {
                    existingKeyword.Education.Institution = dto.Institution;
                }

                if (dto.GraduationYear.HasValue)
                {
                    existingKeyword.Education.GraduationYear = dto.GraduationYear.Value;
                }
            }

            // Save changes
            var saved = await SaveChangesAsync();

            if (saved)
            {
                return existingKeyword;
            }

            return null;
        }


        public async Task<Keyword?> GetByIdAsync(Guid id)
        {
            // null checks
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id");
            }

            // get keyword
            var keyword = await _dbContext.Keywords.FirstOrDefaultAsync(x => x.KeywordId == id);

            return keyword;
        }

        public Task<Education?> CreateEducation(Guid keywordId, Education education)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEducation(Guid keyword, Education education)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Keyword>?> GetAllAsync()
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
