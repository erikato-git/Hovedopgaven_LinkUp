using LinkUp_REST_API.Models.Pending;


namespace LinkUp_REST_API.Repositories.Interfaces.Pending
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
