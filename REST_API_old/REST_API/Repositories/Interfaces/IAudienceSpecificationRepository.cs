using REST_API.Models;

namespace REST_API.Repositories.Interfaces
{
    public interface IAudienceSpecificationRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(AudienceSpecification audienceSpecification);
        Task SaveChangesAsync();
    }
}
