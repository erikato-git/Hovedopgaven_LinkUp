using System.Security.Claims;

namespace REST_API.Controllers.IHelpers
{
    public interface IAuthenticationControllerHelper
    {
        String ExtractUserAccountId(ClaimsPrincipal user);

    }
}
