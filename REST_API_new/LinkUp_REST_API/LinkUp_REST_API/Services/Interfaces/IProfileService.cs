using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ResultDTO> CreateProfile(ProfileCreateInput dto, string userAccountId);
        Task<ResultDTO> GetProfileById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateProfile(ProfileUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeleteProfileById(Guid id, string userAccountId);

        Task<ResultDTO> SearchQuery(ProfileSearchQueryInput dto);
        Task<ResultDTO> SaveInterestingProfile(Guid profileId, string userAccountId);
    }
}
