using REST_API.Models;

namespace REST_API.Services.IHelpers
{
    // TODO: Consider if it's a good idea to split IAuthentication and IAccountSeriviceHelper
    public interface IAuthentication
    {
        bool CheckAccountIdMatchLoginId(Guid loginId, String UserAccountId);
        Task<Account?> GetAccountFromLoginId(String userAccountId);
    }
}
