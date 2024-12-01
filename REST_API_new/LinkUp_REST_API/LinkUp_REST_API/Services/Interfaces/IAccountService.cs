using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResultDTO> Login(LoginInput dto);
        Task<ResultDTO> Logout();   // Maybe can be handled at controller
        Task<ResultDTO> CreateAccount(AccountCreateInput dto);
        Task<ResultDTO> GetAccountById(Guid id, string userAccountId);
        Task<ResultDTO> UpdateAccount(AccountUpdateInput dto, string userAccountId);
        Task<ResultDTO> DeleteAccountById(Guid id, string userAccountId);
    }
}
