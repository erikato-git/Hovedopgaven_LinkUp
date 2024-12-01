using REST_API.DTOs.AccountDomain;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Util;

namespace REST_API.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ResultDTO> CreateProfile(CreateProfileDTO dto, String userAccount);
        Task<ResultDTO> UpdateProfile(UpdateProfileDTO dto, String userAccountId);
        Task<ResultDTO> DeleteProfile(Guid profileId, String userAccountId);
        Task<ResultDTO> GetProfileById(Guid profileId, String userAccountId);
        Task<ResultDTO> SearchQuery(SearchQueryDTO dto);
        Task<ResultDTO> SaveProfile(Guid profileId, String userAccountId);
    }
}
