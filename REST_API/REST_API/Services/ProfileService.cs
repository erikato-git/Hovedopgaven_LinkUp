using REST_API.DTOs.ProfileDomain;
using REST_API.Util;

namespace REST_API.Services
{
    public class ProfileService : IProfileService
    {
        public Task<ResultDTO> CreateProfile(CreateProfileDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> DeleteProfile(Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> GetProfileById(Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> SearchQuery(SearchQueryDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> UpdateProfile(UpdateProfileDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
