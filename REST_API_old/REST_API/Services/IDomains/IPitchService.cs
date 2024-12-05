using REST_API.DTOs.PitchDomain;
using REST_API.Util;

namespace REST_API.Services.Interfaces
{
    public interface IPitchService
    {
        Task<ResultDTO> SendPitch(SendPitchDTO dto, String userAccountId);
        Task<ResultDTO> GetIncomingPitches(String userAccountId);
        Task<ResultDTO> GetOutcomingPitches(String userAccountId);

    }
}
