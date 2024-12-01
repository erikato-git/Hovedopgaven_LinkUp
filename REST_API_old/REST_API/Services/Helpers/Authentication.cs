using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using REST_API.Models;
using REST_API.Repositories.Interfaces;
using REST_API.Services.IHelpers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace REST_API.Services.Helpers
{
    /*
     * Authentication can be extended with 
     */
    public class Authentication : IAuthentication
    {
        private IAccountRepository _accountRepository;
        private IConfiguration _configuration;

        public Authentication(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public bool CheckAccountIdMatchLoginId(Guid loginId, string UserAccountId)
        {
            return UserAccountId.Equals(loginId.ToString());
        }

        public bool CheckPasswordsMatch(string loginPassword, Account account)
        {
            if (String.IsNullOrEmpty(loginPassword) || String.IsNullOrEmpty(account.Password))
            {
                return false;
            }

            var hashedLoginPassword = HashingPasswordWithSaltUsingSHA256(loginPassword, account.AccountId);

            return hashedLoginPassword.Equals(account.Password);
        }

        /*
         * Reference: Coding Tutorial: Password hashing, https://github.com/JasperKent/Password-Hashing/blob/master/PasswordHashing/Program.cs
         * Declared public static so I can use it in DbInitializer
         */
        public static String HashingPasswordWithSaltUsingSHA256(String unhashedPassword, Guid accountId)
        {
            var salt = accountId.ToString();

            using var sha = SHA256.Create();

            var asBytes = Encoding.Default.GetBytes(unhashedPassword + salt);

            var hashed = sha.ComputeHash(asBytes);

            return Convert.ToBase64String(hashed);
        }

        /*
         * Consider to move this method to a class e.g. called TokenProvider to add more cohesion to AccountServiceHelper
         * Needs to be tested at integration level
         * Reference: https://www.youtube.com/watch?v=6DWJIyipxzw
         */
        public String GenerateJWT(Account account)
        {
            String secretKey = _configuration["JWT:Secret"];

            if (String.IsNullOrEmpty(secretKey) || account == null)
            {
                return "";
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("AccountId",account.AccountId.ToString())
                    // add more claims if needed
                ]),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:ExpirationTimeInMinutes"])),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }


    }
}
