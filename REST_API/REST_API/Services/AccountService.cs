using Azure.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Util;

namespace REST_API.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ResultDTO> Login(LoginDTO dto)
        {
            try
            {
                var account = await _accountRepository.GetAccountByEmail(dto.Email);

                if (account == null)
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_InvalidEmailOrPassword);
                }

                var passwordMatch = CheckPasswordsMatch(dto.Password, account.Password);

                if(passwordMatch)
                {
                    return ResultDTO.SuccesResult(account, "Login successful!");
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_InvalidEmailOrPassword);
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

                if (emailTaken)
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_EmailForAccountAlreadyExist);
                }

                if (!emailTaken)
                {
                    Account? account = null;

                    account = await _accountRepository.AddAsync(dto);

                    // TODO: AddAuthentication

                    if (account != null)
                    {
                        return ResultDTO.SuccesResult(account, "Account has succesfully been created!");
                    }
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
                // TODO: JWT Claim
                // Before I can test it arguments need to come from argument of UpdateAccount() and from fake JWT claims

                //var idsMatch = CheckIdsMatchDummy();

                //if(idsMatch)
                //{
                //    Account? account = null;

                //    account = await _accountRepository.UpdateAsync(dto);

                //    if (account != null)
                //    {
                //        return ResultDTO.SuccesResult(account, "Account has succesfully been updated");
                //    }
                //}

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


            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Couldn't find account");
        }

        public bool CheckPasswordsMatch(string loginDtoPassword, string accountPassword)
        {
            return loginDtoPassword.Equals(accountPassword);
        }

    }
}
