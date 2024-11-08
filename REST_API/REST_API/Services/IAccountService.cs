using REST_API.DTOs;
using REST_API.Util;

namespace REST_API.Services
{
    public interface IAccountService
    {
        ResultDTO Login(LoginDTO dto);
        ResultDTO CreateAccount(CreateAccountDTO dto);
        ResultDTO UpdateAccount(UpdateAccountDTO dto);
        ResultDTO GetAccountById(Guid id);
    }
}
