using REST_API.Models;
using REST_API.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace REST_API.Services.Helpers
{
    public class AccountServiceHelper : IAccountServiceHelper
    {
        private IAccountRepository _accountRepository;

        public AccountServiceHelper(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool CheckPasswordsMatch(string loginPassword, Account account)
        {
            if(String.IsNullOrEmpty(loginPassword) || String.IsNullOrEmpty(account.Password))
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


        public bool CheckAccountIdMatchLoginId(Guid loginId, string UserAccountId)
        {
            throw new NotImplementedException();
        }


        public string GenerateJWT(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountFromLoginId(string userAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
