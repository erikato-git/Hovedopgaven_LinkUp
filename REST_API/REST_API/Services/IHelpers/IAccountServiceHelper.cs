using Microsoft.Identity.Client;
using REST_API.Models;
using REST_API.Services.IHelpers;

namespace REST_API.Services.Helpers
{
    public interface IAccountServiceHelper : IAuthentication 
    {
        String GenerateJWT(Account account);
        bool CheckPasswordsMatch(string password1, string password2);
    }
}
