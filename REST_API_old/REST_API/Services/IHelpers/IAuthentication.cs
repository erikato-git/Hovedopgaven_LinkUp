using REST_API.Models;

namespace REST_API.Services.IHelpers
{
    public interface IAuthentication
    {
        bool CheckAccountIdMatchLoginId(Guid loginId, String UserAccountId);
        String GenerateJWT(Account account);
        bool CheckPasswordsMatch(string password1, Account account);
    }
}
