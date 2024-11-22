using REST_API.Models;
using REST_API.Repositories.Interfaces;

namespace REST_API.Services.Helpers
{
    public class AccountServiceHelper : IAccountServiceHelper
    {
        private IAccountRepository _accountRepository;

        public AccountServiceHelper(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool CheckAccountIdMatchLoginId(Guid loginId, string UserAccountId)
        {
            throw new NotImplementedException();
        }

        public bool CheckPasswordsMatch(string password1, string password2)
        {
            throw new NotImplementedException();
        }

        public string GenerateJWT(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountFromLoginId(string userAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
