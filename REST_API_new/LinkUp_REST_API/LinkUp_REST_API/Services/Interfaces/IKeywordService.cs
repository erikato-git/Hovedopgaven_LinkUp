using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces
{
    public interface IKeywordService
    {
        Task<ResultDTO> CreateKeyword(KeywordCreateUpdateInput createDto, string userAccountId);
        Task<ResultDTO> GetKeywordById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateKeyword(KeywordCreateUpdateInput updateDto, string userAccountId);
        Task<ResultDTO> DeleteKeywordById(Guid id, string userAccountId);
    }
}
