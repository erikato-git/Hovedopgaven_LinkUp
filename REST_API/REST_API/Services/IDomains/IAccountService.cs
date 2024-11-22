using REST_API.DTOs.AccountDomain;
using REST_API.Util;

namespace REST_API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResultDTO> Login(LoginDTO dto);
        Task<ResultDTO> CreateAccount(CreateAccountDTO dto);
        Task<ResultDTO> UpdateAccount(UpdateAccountDTO dto, String userAccountId);
        Task<ResultDTO> GetAccountById(Guid id, String userAccountId);
        Task<ResultDTO> DeleteAccountById(Guid id, String userAccountId);
    }
}
