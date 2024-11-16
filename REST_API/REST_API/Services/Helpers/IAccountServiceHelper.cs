using Microsoft.Identity.Client;
using REST_API.Models;

namespace REST_API.Services.Helpers
{
    public interface IAccountServiceHelper
    {
        bool CheckAccountIdMatchLoginId(Guid accountId);
        String GenerateJWT(Account account);
        bool CheckPasswordsMatch(string password1, string password2);
    }
}
