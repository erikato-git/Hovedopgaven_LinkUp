using LinkUp_REST_API.Models.Pending;


namespace REST_API.Repositories.Interfaces
{
    public interface IAudienceSpecificationRepository
    {
        // Common repository methods
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task UpdateAsync(AudienceSpecification audienceSpecification);
        Task SaveChangesAsync();
    }
}
