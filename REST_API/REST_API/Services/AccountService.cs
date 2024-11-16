using Azure.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Services.Helpers;
using REST_API.Util;

namespace REST_API.Services
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
                var account = await _accountRepository.GetAccountByEmail(dto.Email);

                if (account != null)
                {
                    var checkPasswords = _accountServiceHelper.CheckPasswordsMatch(dto.Password, account.Password);     // TODO: make sure account.Password is hashed

                    if (checkPasswords)
                    {
                        var JWT = _accountServiceHelper.GenerateJWT(account);

                        if (JWT != null)
                        {
                            return ResultDTO.SuccesResult(new LoginResponseDTO { Account = account, JWT = JWT }, "User has succesfully logged in!");
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
                var emailTaken = await _accountRepository.doesEmailForAccountExist(dto.Email);

                if (!emailTaken)
                {
                    var account = await _accountRepository.AddAsync(dto);

                    if (account != null)
                    {
                        var JWT = _accountServiceHelper.GenerateJWT(account);

                        return ResultDTO.SuccesResult(new LoginResponseDTO { Account = account, JWT = JWT}, "Account has succesfully been created!");
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
                var authorization = _accountServiceHelper.CheckAccountIdMatchLoginId(dto.AccountId);

                if (authorization)
                {
                    var account = await _accountRepository.UpdateAsync(dto);
                    
                    if(account != null)
                    {
                        return ResultDTO.SuccesResult(account, "Account has succesfully been updated");
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
                var authorization = _accountServiceHelper.CheckAccountIdMatchLoginId(id);

                if(authorization)
                {
                    var account = await _accountRepository.GetByIdAsync(id);

                    if(account != null)
                    {
                        return ResultDTO.SuccesResult(account, $"Received Account {id}");
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
                var authorization = _accountServiceHelper.CheckAccountIdMatchLoginId(id);

                if (authorization)
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
                    return ResultDTO.FailureResult(ErrorMessages.AccountSerivce_DeleteAccountById_CannotDeleteAnotherPersonsAccound);
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
