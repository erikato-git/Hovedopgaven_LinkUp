using REST_API.Models;

namespace REST_API.Services.IHelpers
{
    public interface IAuthentication
    {
        bool CheckAccountIdMatchLoginId(Guid loginId, String UserAccountId);
        Task<Account?> GetAccountFromLoginId(String userAccountId);
    }
}
