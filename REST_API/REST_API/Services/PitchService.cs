using REST_API.DTOs;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Services
{
    public class PitchService : IPitchService
    {
        public Task<ResultDTO> GetIncomingPitches()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> GetOutcomingPitches()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> SendPitch(SendPitchDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
