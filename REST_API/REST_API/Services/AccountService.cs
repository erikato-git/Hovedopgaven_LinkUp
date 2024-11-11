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

                // Errors

                if (result.Message.Equals(ErrorMessages.AccountRepository_FindAccountByEmailAndPassword_EmailAndPasswordDontMatch))
                {
                    return ResultDTO.FailureResult(ErrorMessages.AccountService_Login_401InvalidCredentials);       // generic reponse 
                }


                // Success

                return ResultDTO.SuccesResult(null, "");
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
                /*
                 * Helpful information for users, not helpful for attackers
                 */
                return ResultDTO.FailureResult("Login failed");         // default Error-Message
            }
        }


        public ResultDTO CreateAccount(CreateAccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public ResultDTO GetAccountById(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResultDTO UpdateAccount(UpdateAccountDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
