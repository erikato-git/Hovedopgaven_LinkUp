using REST_API.DTOs.ProfileDomain;
using REST_API.Models;

namespace REST_API.Services.Helpers
{
    public interface IProfileServiceHelper
    {
        Task<Profile?> GetProfileFromAccount(Account? account, Guid? profileId);
        Task<IEnumerable<ProfileSearchResponseDTO>?> SearchProfiles(SearchQueryDTO searchQuery);
        void SaveImageToCloudinary(IFormFile file);
        void DeleteImageFromCloudinary(Guid id);
    }
}
