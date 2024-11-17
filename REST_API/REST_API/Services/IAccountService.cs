using REST_API.DTOs.AccountDomain;
using REST_API.Util;

namespace REST_API.Services
{
    public interface IAccountService
    {
        Task<ResultDTO> Login(LoginDTO dto);
        Task<ResultDTO> CreateAccount(CreateAccountDTO dto);
        Task<ResultDTO> UpdateAccount(UpdateAccountDTO dto);
        Task<ResultDTO> GetAccountById(Guid id);
        Task<ResultDTO> DeleteAccountById(Guid id);
    }
}
