using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.DTOs.Responses.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Services.Interfaces.Completed
{
    public interface IProfileServiceHelper
    {
        Task<bool> SaveMedia(IFormFile file, Profile profile);
        Task<bool> DeleteMedia(Guid mediaId);
        Task<IEnumerable<ProfileSearchQueryOutput>?> QuerySearchedProfiles(ProfileSearchQueryInput dto);

    }
}
