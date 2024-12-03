using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Completed
{
    public interface IProfileService
    {
        Task<ResultDTO> CreateProfile(ProfileCreateInput dto, string userAccountId);
        Task<ResultDTO> GetProfileById(Guid id);
        Task<ResultDTO> UpdateProfile(ProfileUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeleteProfileById(Guid id, string userAccountId);

        Task<ResultDTO> SearchQuery(ProfileSearchQueryInput dto);
        Task<ResultDTO> AddProfileToFavorites(Guid profileId, string userAccountId);
        Task<ResultDTO> RemoveProfileFromFavorites(Guid profileId, string userAccountId);
    }
}
