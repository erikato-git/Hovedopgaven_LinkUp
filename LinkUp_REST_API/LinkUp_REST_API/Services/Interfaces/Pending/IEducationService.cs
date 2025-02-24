using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Pending
{
    public interface IEducationService
    {
        Task<ResultDTO> CreateEducation(object createDto, string userAccountId);
        Task<ResultDTO> GetEducationById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateEducation(object updateDto, string userAccountId);
        Task<ResultDTO> DeleteEducationById(Guid id, string userAccountId);
    }
}
