using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IKeyword
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(Keyword keyword);
        Task SaveChangesAsync();

        // Composition
        Task CreateEducation(Keyword keyword, Education education);
        Task DeleteEducation(Keyword keyword, Education education);

    }
}
