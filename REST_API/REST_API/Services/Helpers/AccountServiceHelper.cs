using REST_API.Models;

namespace REST_API.Services.Helpers
{
    public class AccountServiceHelper : IAccountServiceHelper
    {
        public bool CheckAccountIdMatchLoginId(Guid accountId)
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

        public Task<Account?> GetAccountFromLoginId()
        {
            throw new NotImplementedException();
        }
    }
}
