using REST_API.Models;

namespace REST_API.Repositories
{
    public interface IAudienceSpecification
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(AudienceSpecification audienceSpecification);
        Task SaveChangesAsync();
    }
}
