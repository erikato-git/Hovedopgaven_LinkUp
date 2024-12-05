using REST_API.DTOs.AccountDomain;
using REST_API.Repositories.Interfaces;
using REST_API.Services.IHelpers;
using REST_API.Services.Interfaces;
using REST_API.Util;
using REST_API.Util.Mapper;

namespace REST_API.Services.Domains
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

        public async Task<ResultDTO> Login(LoginDTO dto)
        {
            try
            {
                var accountFound = await _accountRepository.GetAccountByEmailAsync(dto.Email);

                if (accountFound != null)
                {
                    var passwordsMatch = _authentication.CheckPasswordsMatch(dto.Password, accountFound);

                    if (passwordsMatch)
                    {
                        var JWT = _authentication.GenerateJWT(accountFound);

                        if (!string.IsNullOrEmpty(JWT))
                        {
                            return ResultDTO.SuccesResult(new LoginResponseDTO { Account = accountFound, JWT = JWT }, "User has succesfully logged in!");
                        }
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountService_Login_InvalidEmailOrPassword);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_Login_InvalidEmailOrPassword);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Login failed");         // default Error-Message
        }


        public async Task<ResultDTO> CreateAccount(CreateAccountDTO dto)
        {
            try
            {
                var emailTaken = await _accountRepository.doesEmailForAccountExistAsync(dto.Email);

                if (!emailTaken)
                {
                    var newAccount = AccountMapper.MapToAccount(dto);

                    var createdAccount = await _accountRepository.AddAsync(newAccount);

                    if (createdAccount != null)
                    {
                        var JWT = _authentication.GenerateJWT(createdAccount);

                        return ResultDTO.SuccesResult(new LoginResponseDTO { Account = createdAccount, JWT = JWT }, "Account has succesfully been created!");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_CreateAccount_CreateAccountFailed);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_CreateAccount_EmailForAccountAlreadyExist);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Create account failed");
        }

        public async Task<ResultDTO> UpdateAccount(UpdateAccountDTO dto, String userAccountId)
        {
            try
            {
                var hasAuthorization = _authentication.CheckAccountIdMatchLoginId(dto.AccountId, userAccountId);

                if (hasAuthorization)
                {
                    var parsedGuid = Guid.Parse(userAccountId);
                    var existingAccount = await _accountRepository.GetAccountByIdAsync(parsedGuid);

                    if (existingAccount == null)
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_UpdateAccount_LoggedInAccountDoesNotExist);
                    }

                    var updatedAccount = AccountMapper.MapUpdateAccountDTOToAccount(dto, existingAccount);

                    var savedUpdatedAccount = await _accountRepository.UpdateAsync(updatedAccount);

                    if (savedUpdatedAccount != null)
                    {
                        return ResultDTO.SuccesResult(savedUpdatedAccount, "Account has succesfully been updated");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Update account failed");
        }

        public async Task<ResultDTO> GetAccountById(Guid id, String userAccountId)
        {
            try
            {
                var hasAuthorization = _authentication.CheckAccountIdMatchLoginId(id, userAccountId);

                if (hasAuthorization)
                {
                    var accountFound = await _accountRepository.GetAccountByIdAsync(id);

                    if (accountFound != null)
                    {
                        return ResultDTO.SuccesResult(accountFound, $"Received Account {id}");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_GetAccountById_FailedToRetrieveAccountInternalServerError);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_GetAccountById_CannotRetrieveAnothersAccount);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Couldn't get account");
        }

        public async Task<ResultDTO> DeleteAccountById(Guid id, String userAccountId)
        {
            try
            {
                var hasAuthorization = _authentication.CheckAccountIdMatchLoginId(id, userAccountId);

                if (hasAuthorization)
                {
                    var accountDeleted = await _accountRepository.DeleteAsync(id);

                    if (accountDeleted)
                    {
                        return ResultDTO.SuccesResult(accountDeleted, "Account has succesfully been deleted");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_DeleteAccountById_CannotDeleteAnotherPersonsAccount);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Couldn't delete account");
        }
    }
}
