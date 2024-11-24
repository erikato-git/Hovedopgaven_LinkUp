using REST_API.Models;
using REST_API.Repositories.Interfaces;
using REST_API.Services.IHelpers;

namespace REST_API.Services.Helpers
{
    /*
     * Authentication can be extended with 
     */
    public class Authentication : IAuthentication
    {
        private IAccountRepository _accountRepository;
        private IConfiguration _configuration;

        public Authentication(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public bool CheckAccountIdMatchLoginId(Guid loginId, string UserAccountId)
        {
            return UserAccountId.Equals(loginId.ToString());
        }

        public Task<Account?> GetAccountFromLoginId(string userAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
