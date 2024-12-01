using REST_API.Controllers.IHelpers;
using System.Security.Claims;

namespace REST_API.Controllers.Helpers
{
    public class AccountControllerHelper : IAccountControllerHelper
    {
        public String ExtractUserAccountId(ClaimsPrincipal user)
        {
            //if (user == null)
            //    throw new ArgumentNullException(nameof(user));

            //var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier); // Or a custom claim type
            //if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            //    throw new InvalidOperationException("The user ID claim is missing or invalid.");

            //return userId;
            return "";
        }
    }
}
