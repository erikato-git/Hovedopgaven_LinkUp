using REST_API.DTOs.AccountDomain;
using REST_API.DTOs.ProfileDomain;
using REST_API.Util;

namespace REST_API.Services
{
    public interface IProfileService
    {
        Task<ResultDTO> CreateProfile(CreateProfileDTO dto);
        Task<ResultDTO> UpdateProfile(UpdateProfileDTO dto);
        Task<ResultDTO> DeleteProfile(Guid profileId);
        Task<ResultDTO> GetProfileById(Guid profileId);
        Task<ResultDTO> SearchQuery(SearchQueryDTO dto);
    }
}
