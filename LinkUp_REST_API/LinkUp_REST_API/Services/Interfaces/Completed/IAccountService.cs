using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Completed
{
    public interface IAccountService
    {
        Task<ResultDTO> Login(LoginInput dto);
        Task<ResultDTO> CreateAccount(AccountCreateInput dto);
        Task<ResultDTO> GetExternalAccountById(Guid id, string userAccountId);
        Task<ResultDTO> GetOwnAccount(string userAccountId);
        Task<ResultDTO> UpdateAccount(AccountUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeleteOwnAccount(AccountDeleteInput dto, string userAccountId);
    }
}
