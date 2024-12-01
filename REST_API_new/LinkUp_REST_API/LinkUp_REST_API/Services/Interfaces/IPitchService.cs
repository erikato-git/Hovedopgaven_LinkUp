using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces
{
    public interface IPitchService
    {
        Task<ResultDTO> CreatePitch(PitchCreateInput dto, string userAccountId);
        Task<ResultDTO> GetProfileById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateProfile(ProfileUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeleteProfileById(Guid id, string userAccountId);


        Task<ResultDTO> GetIncomingPitches(string userAccountId);
        Task<ResultDTO> GetOutcomingPitches(string userAccountId);

    }
}
