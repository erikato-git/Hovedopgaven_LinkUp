using REST_API.DTOs;
using REST_API.Util;

namespace REST_API.Repositories
{
    public interface IAccountRepository
    {
        ResultDTO FindAccountByEmailAndPassword(LoginDTO dto);      // login
        ResultDTO CreateAccount(CreateAccountDTO dto);
        ResultDTO FindAccountByEmail(String email);
        ResultDTO UpdateAccount(UpdateAccountDTO dto);
        ResultDTO FindAccountById(Guid guid);
    }
}
