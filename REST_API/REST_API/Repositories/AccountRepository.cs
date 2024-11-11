using REST_API.DTOs;
using REST_API.Util;

namespace REST_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public ResultDTO CreateAccount(CreateAccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public ResultDTO FindAccountByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ResultDTO FindAccountByEmailAndPassword(LoginDTO dto)
        {
            throw new NotImplementedException();
        }

        public ResultDTO FindAccountById(Guid guid)
        {
            throw new NotImplementedException();
        }

        public ResultDTO UpdateAccount(UpdateAccountDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
