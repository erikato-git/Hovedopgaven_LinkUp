using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces
{
    public interface IPitchService
    {
        Task<ResultDTO> CreatePitch(PitchCreateInput dto, string userAccountId);
        Task<ResultDTO> GetPitchById(Guid id, string userAccountId);
        Task<ResultDTO> DeletePitchById(Guid id, string userAccountId);


        Task<ResultDTO> GetAllAssociatedPithes(string userAccountId);

    }
}
