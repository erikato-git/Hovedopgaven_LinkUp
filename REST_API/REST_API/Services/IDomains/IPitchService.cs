using REST_API.DTOs;
using REST_API.DTOs.ProfileDomain;
using REST_API.Util;

namespace REST_API.Services.Interfaces
{
    public interface IPitchService
    {
        Task<ResultDTO> SendPitch(SendPitchDTO dto);
        Task<ResultDTO> GetIncomingPitches();
        Task<ResultDTO> GetOutcomingPitches();

    }
}
