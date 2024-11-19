using Azure.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Services.Domains
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        private IAccountServiceHelper _accountServiceHelper;

        public AccountService(IAccountRepository accountRepository, IAccountServiceHelper accountServiceHelper)
        {
            _accountRepository = accountRepository;
            _accountServiceHelper = accountServiceHelper;
        }

        public async Task<ResultDTO> Login(LoginDTO dto)
        {
            try
            {
                var accountFound = await _accountRepository.GetAccountByEmailAsync(dto.Email);

                if (accountFound != null)
                {
                    var passwordsMatch = _accountServiceHelper.CheckPasswordsMatch(dto.Password, accountFound.Password);     // TODO: make sure account.Password is hashed

                    if (passwordsMatch)
                    {
                        var JWT = _accountServiceHelper.GenerateJWT(accountFound);

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
                    var createdAccount = await _accountRepository.AddAsync(dto);

                    if (createdAccount != null)
                    {
                        var JWT = _accountServiceHelper.GenerateJWT(createdAccount);

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

        public async Task<ResultDTO> UpdateAccount(UpdateAccountDTO dto)
        {
            try
            {
                var hasAuthorization = _accountServiceHelper.CheckAccountIdMatchLoginId(dto.AccountId);

                if (hasAuthorization)
                {
                    var updatedAccount = await _accountRepository.UpdateAsync(dto);

                    if (updatedAccount != null)
                    {
                        return ResultDTO.SuccesResult(updatedAccount, "Account has succesfully been updated");
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

        public async Task<ResultDTO> GetAccountById(Guid id)
        {
            try
            {
                var hasAuthorization = _accountServiceHelper.CheckAccountIdMatchLoginId(id);

                if (hasAuthorization)
                {
                    var accountFound = await _accountRepository.GetByIdAsync(id);

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

        public async Task<ResultDTO> DeleteAccountById(Guid id)
        {
            try
            {
                var hasAuthorization = _accountServiceHelper.CheckAccountIdMatchLoginId(id);

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
