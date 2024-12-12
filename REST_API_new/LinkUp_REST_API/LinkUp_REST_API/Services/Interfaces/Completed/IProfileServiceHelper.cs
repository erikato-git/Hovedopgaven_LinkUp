using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.DTOs.Responses.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Services.Interfaces.Completed
{
    public interface IProfileServiceHelper
    {
        Task<Media?> SaveMedia(IFormFile file, Profile profile);
        Task<bool> DeleteMedia(string mediaId, Profile profile);
        Task<IEnumerable<ProfileSearchQueryOutput>?> QuerySearchedProfiles(ProfileSearchQueryInput dto);

    }
}
