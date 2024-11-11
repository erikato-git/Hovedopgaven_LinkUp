using REST_API.DTOs;
using REST_API.Util;

namespace REST_API.Repositories
{
    public interface IAccountRepository
    {
        ResultDTO FindAccountByEmailAndPassword(LoginDTO dto);      // login
    }
}
