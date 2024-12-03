using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using LinkUp_REST_API.Util.Mapper.Completed;
using Microsoft.IdentityModel.Tokens;

namespace LinkUp_REST_API.Services.Completed
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        private IAuthentication _authentication;

        public AccountService(IAccountRepository accountRepository, IAuthentication authentication)
        {
            _accountRepository = accountRepository;
            _authentication = authentication;
        }


        public async Task<ResultDTO> DeleteOwnAccount(string userAccountId)
        {
            if (string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Input is invalid");
            }

            var accountDeleted = await _accountRepository.DeleteAsync(Guid.Parse(userAccountId));

            if (!accountDeleted)
            {
                return ResultDTO.Failure(500, $"Remove account {userAccountId} failed due to internal server error");
            }

            return ResultDTO.Succes(accountDeleted, 200, $"Account {userAccountId} deleted");
        }


        public async Task<ResultDTO> GetOwnAccount(string userAccountId)
        {
            if (string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Input is invalid");
            }

            // Get Account
            var ownAccount = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (ownAccount == null)
            {
                return ResultDTO.Failure(404, $"Account {userAccountId} was not found");
            }

            return ResultDTO.Succes(ownAccount, 200, "Own account fetched");
        }

        public async Task<ResultDTO> GetExternalAccountById(Guid accountId, string userAccountId)
        {
            if (string.IsNullOrEmpty(accountId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Inputs are invalid");
            }

            // Check if accountId match userAccountId
            var idsMatch = accountId.ToString().Equals(userAccountId);

            if (idsMatch)
            {
                return ResultDTO.Failure(403, "You cannot access your own account from this endpoint");
            }

            // Get Account
            var getAccount = await _accountRepository.GetByIdAsync(accountId);

            if (getAccount == null)
            {
                return ResultDTO.Failure(404, $"Account {accountId} does not exist");
            }

            // Map to AccountExternalGetOutput
            var externalAccount = AccountMapper.ToExternalDetailsOutput(getAccount);


            return ResultDTO.Succes(externalAccount, 200, $"Account {accountId} has been fetched");
        }

        public async Task<ResultDTO> Login(LoginInput dto)
        {
            if (dto == null)
            {
                return ResultDTO.Failure(400, "No input");
            }

            // check email exist
            var accountExist = await _accountRepository.GetAccountByEmailAsync(dto.Email);

            if (accountExist == null)
            {
                return ResultDTO.Failure(400, "email or password is invalid");      // generic error-message
            }

            // check hashed passwords match
            var passwordsMatch = _authentication.CheckPasswordsMatch(dto.Password, accountExist);

            if (!passwordsMatch)
            {
                return ResultDTO.Failure(400, "email or password is invalid");
            }

            // generate JWT token
            var JWT = _authentication.GenerateJWT(accountExist);
            if (string.IsNullOrEmpty(JWT))
            {
                return ResultDTO.Failure(500, "Could not generate JWT token");
            }

            // map JWT and accountExist to 'loginOutput'
            var loginOutput = AccountMapper.MapToLoginOutput(accountExist, JWT);

            return ResultDTO.Succes(loginOutput, 200, "You're now logged in");
        }



        public async Task<ResultDTO> CreateAccount(AccountCreateInput dto)
        {
            if (dto == null)
            {
                return ResultDTO.Failure(400, "Input is null");
            }

            // Check new user is min. 13 (GDPR)
            var minAge = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-13));

            if (dto.BirthDate.Year >= minAge.Year)
            {
                return ResultDTO.Failure(409, "User must be min. 13 years old");
            }

            // Check email already exist
            var accountExist = await _accountRepository.GetAccountByEmailAsync(dto.Email);

            if (accountExist != null)
            {
                return ResultDTO.Failure(409, "Email already exist");
            }

            // Create Account
            var newAccount = AccountMapper.MapToAccount(dto);

            var createdAccount = await _accountRepository.AddAsync(newAccount);

            if (createdAccount == null)
            {
                return ResultDTO.Failure(500, "Failed to create account due to internal server error");
            }

            // generate JWT token
            var JWT = _authentication.GenerateJWT(createdAccount);
            if (string.IsNullOrEmpty(JWT))
            {
                return ResultDTO.Failure(500, "Could not generate JWT token");
            }

            // map JWT and accountExist to 'loginOutput'
            var loginOutput = AccountMapper.MapToLoginOutput(createdAccount, JWT);


            return ResultDTO.Succes(loginOutput, 201, "Account has been created");
        }

        public async Task<ResultDTO> UpdateAccount(AccountUpdateInput dto, string userAccountId)
        {
            if (dto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Update account info or logged in account is null");
            }

            // Check AccountId in dto match userAccountId
            if (!dto.AccountId.ToString().Equals(userAccountId))
            {
                return ResultDTO.Failure(403, "You don't have authorization to update this account");
            }

            // if email is changed: check if it's changed to an already existing email
            if (!string.IsNullOrEmpty(dto.Email))
            {
                var accountExist = await _accountRepository.GetAccountByEmailAsync(dto.Email);

                if (accountExist != null)
                {
                    return ResultDTO.Failure(409, "You cannot change your email to an email that already exist");
                }
            }

            // if birthdate is changed: check it's not under 13
            if (dto.BirthDate != null)
            {
                // Check new user is min. 13 (GDPR)
                var minAge = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-13));

                if (dto.BirthDate?.Year >= minAge.Year)
                {
                    return ResultDTO.Failure(409, "User must be min. 13 years old");
                }
            }

            // Find exising account
            var existingAccount = await _accountRepository.GetByIdAsync(dto.AccountId);
            if (existingAccount == null)
            {
                return ResultDTO.Failure(404, "Could not find target account");
            }

            // Update account
            var updatedAccount = await _accountRepository.UpdateAsync(dto);

            if (updatedAccount == null)
            {
                return ResultDTO.Failure(500, "Failed to update account due to internal server error");
            }

            return ResultDTO.Succes(updatedAccount, 200, "Account has been updated");
        }



        public Task<ResultDTO> Logout()
        {
            throw new NotImplementedException();
        }

    }
}
