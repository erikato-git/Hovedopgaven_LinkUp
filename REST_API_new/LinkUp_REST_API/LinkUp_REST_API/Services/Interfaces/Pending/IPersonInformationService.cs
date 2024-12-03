using LinkUp_REST_API.DTOs.Requests.Pending;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Pending
{
    public interface IPersonInformationService
    {
        Task<ResultDTO> CreatePersonInformation(PersonInformationCreateInput dto, string userAccountId);
        Task<ResultDTO> GetPersonInformationById(Guid id, string userAccountId);
        Task<ResultDTO> UpdatePersonInformation(PersonInformationUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeletePersonInformationById(Guid id, string userAccountId);
    }
}
