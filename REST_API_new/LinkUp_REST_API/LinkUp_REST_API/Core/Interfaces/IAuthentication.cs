using LinkUp_REST_API.Models;
using System.Security.Claims;

namespace LinkUp_REST_API.Core.Interfaces
{
    public interface IAuthentication
    {
        string? GetCurrentUserId(ClaimsPrincipal claimsPrincipal);
        bool CheckAccountIdMatchLoginId(Guid loginId, String UserAccountId);
        string? GenerateJWT(Account account);
        bool CheckPasswordsMatch(string password1, Account account);
    }
}
