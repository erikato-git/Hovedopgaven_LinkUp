using Microsoft.Identity.Client;

namespace REST_API.Services.Helpers
{
    public interface IAccountServiceHelper
    {
        bool CheckIdsMatch(Guid id1, Guid id2);
        bool AddAuthentication(Guid id);
        bool CheckPasswordsMatch(string password1, string password2);
    }
}
