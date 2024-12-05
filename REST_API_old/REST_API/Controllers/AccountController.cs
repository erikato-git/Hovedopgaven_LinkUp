using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_API.Controllers.IHelpers;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private IAccountControllerHelper _accountControllerHelper;

        public AccountController(IAccountService accountService, IAccountControllerHelper accountControllerHelper)
        {
            _accountService = accountService;
            _accountControllerHelper = accountControllerHelper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.Login(dto);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [AllowAnonymous]
        [HttpPost("createAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]             // JWT token
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.CreateAccount(dto);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Created("", result.Data);
            }

            if (result.Message.Equals(ErrorMessages.AccountService_CreateAccount_EmailForAccountAlreadyExist))
            {
                return Conflict(result.Message);
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_CreateAccount_CreateAccountFailed))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


            return BadRequest(result.Message);
        }


        [Authorize]
        [HttpPut("updateAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _accountControllerHelper.ExtractUserAccountId(User);

            var result = await _accountService.UpdateAccount(dto, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_UpdateAccount_YouMustBeSignedInBeforeYouCanUpdateYourAccount))
            {
                return Unauthorized(result.Message);
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_UpdateAccount_UpdateAccountFailed))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


            return BadRequest(result.Message);
        }


        [Authorize]
        [HttpGet("getAccount/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountById([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _accountControllerHelper.ExtractUserAccountId(User);

            var result = await _accountService.GetAccountById(id, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_GetAccountById_AccountNotFound))
            {
                return NotFound(result.Message);
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_GetAccountById_YouCannotAccessAnotherAccount))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }


            return BadRequest(result.Message);
        }


        [Authorize]
        [HttpDelete("deleteAccount/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAccountById([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _accountControllerHelper.ExtractUserAccountId(User);

            var result = await _accountService.DeleteAccountById(id, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return NoContent();
            }

            if (result.Message.Equals(ErrorMessages.AccountSerivce_DeleteAccount_DeleteAccountFailed))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


            return BadRequest(result.Message);
        }

    }
}
