using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.DTOs.Requests.Completed;

namespace LinkUp_REST_API.Repositories.Interfaces.Completed
{
    public interface IProfileRepository
    {
        // Common repository methods
        Task<Profile?> GetByIdAsync(Guid id);
        Task<Profile?> UpdateAsync(ProfileUpdateInput dto);
        Task<bool> SaveChangesAsync();


        // Custom methods
        Task<IEnumerable<Profile>?> GetProfilesForAccountAsync(Guid accountId);


        // Composition
        Task<Profile?> CreateProfileAsync(Guid accountId, Profile profile);
        Task<bool> DeleteProfileAsync(Guid accountId, Profile profile);


    }
}
