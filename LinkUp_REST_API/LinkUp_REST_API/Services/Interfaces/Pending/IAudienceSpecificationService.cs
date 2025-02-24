using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Pending
{
    public interface IAudienceSpecificationService
    {
        Task<ResultDTO> CreateAudienceSpecification(object createDto, string userAccountId);
        Task<ResultDTO> GetAudienceSpecificationById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateAudienceSpecification(object updateDto, string userAccountId);
        Task<ResultDTO> DeleteAudienceSpecificationById(Guid id, string userAccountId);
    }
}
