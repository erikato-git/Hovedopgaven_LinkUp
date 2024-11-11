using Azure.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using REST_API.DTOs;
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

        public ResultDTO Login(LoginDTO dto)
        {
            try
            {
                var result = _accountRepository.FindAccountByEmailAndPassword(dto);

                // Success

                if (result.isSuccess && result.Data != null)
                {
                    // TODO: add JWT

                    return ResultDTO.SuccesResult(result.Data, "Data with JWT");
                }

                // Errors

                if (result.Message.Equals(ErrorMessages.AccountRepository_FindAccountByEmailAndPassword_EmailAndPasswordDontMatch))
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_Login_401InvalidCredentials);       // generic reponse 
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            /*
             *  Helpful information for users, not helpful for attackers
             */
            return ResultDTO.FailureResult("Login failed");         // default Error-Message
        }


        public ResultDTO CreateAccount(CreateAccountDTO dto)
        {
            try
            {
                var result = _accountRepository.CreateAccount(dto);

                // Success

                if (result.isSuccess && result.Data != null)
                {
                    return ResultDTO.SuccesResult(result.Data, "Account has succesfully been created");
                }

                /*
                 * ErrorMessages
                 * pros: I can expose internal errors-messages and proces the logic-flow after that
                 * cons: some of the error-messages are pretty similar
                 */
                // Errors
                if(result.Message.Equals(ErrorMessages.AccountRepository_CreateAccount_EmailAlreadyTaken))
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_CreateAccount_409InvalidEmail);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Create account failed");
        }

        public ResultDTO UpdateAccount(UpdateAccountDTO dto)
        {
            try
            {
                // TODO: check if AccountId in UpdateAccountDto match AccountId from User. If an attacker hijacks the JWT and uses the AccountId in his / her UpdateAccountDTO ... Find out how to implement proper authorization, maybe add more checks like password


                var result = _accountRepository.UpdateAccount(dto);

                // Success

                if (result.isSuccess && result.Data != null)
                {
                    return ResultDTO.SuccesResult(result.Data, "Account has succesfully been updated");
                }

                // Errors
                if (result.Message.Equals(ErrorMessages.AccountRepository_CreateAccount_EmailAlreadyTaken))
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_CreateAccount_409InvalidEmail);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Update account failed");
        }

        public ResultDTO GetAccountById(Guid id)
        {
            try
            {
                // TODO: check if AccountId in UpdateAccountDto match AccountId from User. If an attacker hijacks the JWT and uses the AccountId in his / her UpdateAccountDTO ... Find out how to implement proper authorization, maybe add more checks like password


                var result = _accountRepository.FindAccountById(id);

                // Success

                if (result.isSuccess && result.Data != null)
                {
                    return ResultDTO.SuccesResult(result.Data, "Account is found");
                }

                // Errors

                if (result.Message.Equals(ErrorMessages.AccountRepository_FindAccountById_AccountWasNotFound))
                {
                    return ResultDTO.FailureResult(result.Message);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Couldn't find account");
        }

    }
}
