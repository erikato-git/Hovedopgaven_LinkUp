using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Util;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LinkUp_REST_API.Core
{
    public class Authentication : IAuthentication
    {
        private readonly JwtSettings _jwtSettings;

        public Authentication(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string? GenerateJWT(Account account)
        {
            if (account == null || string.IsNullOrEmpty(_jwtSettings.Secret))
            {
                return string.Empty;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("AccountId", account.AccountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }


        public bool CheckPasswordsMatch(string loginPassword, Account account)
        {
            if (string.IsNullOrEmpty(loginPassword) || string.IsNullOrEmpty(account.Password))
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
        public static string HashingPasswordWithSaltUsingSHA256(string unhashedPassword, Guid accountId)
        {
            var salt = accountId.ToString();

            using var sha = SHA256.Create();

            var asBytes = Encoding.Default.GetBytes(unhashedPassword + salt);

            var hashed = sha.ComputeHash(asBytes);

            return Convert.ToBase64String(hashed);
        }


        public string? GetCurrentUserId(ClaimsPrincipal claimsPrincipal)
        {
            var accountIdClaim = claimsPrincipal?.Claims
                .FirstOrDefault(c => c.Type == "AccountId")?.Value;

            return accountIdClaim ?? string.Empty;
        }

    }
}
