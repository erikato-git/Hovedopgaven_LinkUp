using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LinkUp_REST_API.Controllers.Completed
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        private IAuthentication _authentication;

        public AccountsController(IAccountService accountService, IAuthentication authentication)
        {
            _accountService = accountService;
            _authentication = authentication;
        }

        /*
         * TODO: Consider to move 'login' and 'logout' to its own controller-class
         */

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Login([FromBody] LoginInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isUserLoggedIn = _authentication.GetCurrentUserId(User);

                if (!string.IsNullOrEmpty(isUserLoggedIn))
                {
                    return BadRequest("Your are already logged in");
                }

                var result = await _accountService.Login(dto);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Login failed");
            }
        }


        [AllowAnonymous]
        [HttpPost("createAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isUserLoggedIn = _authentication.GetCurrentUserId(User);

                if (!string.IsNullOrEmpty(isUserLoggedIn))
                {
                    return BadRequest("You cannot create an account when you are logged in");
                }

                var result = await _accountService.CreateAccount(dto);

                if (result.isSucces)
                {
                    // TODO: insert 'GetByAccountId' path in ""
                    return Created("", result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Create account failed");
            }
        }


        [Authorize]
        [HttpPut("updateAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized("You must to be logged in before you can update your account");
                }

                var result = await _accountService.UpdateAccount(dto, loggedInAccount);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                // TODO: log-info: AccountId (loggedInUser), UTC.Now, ex.stack-trace 
                return BadRequest("Updated account failed");
            }
        }


        [Authorize]
        [HttpGet("getExternalAccountById/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExternalAccountById([FromQuery] Guid accountId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized("You must to be logged in before you can access this account");
                }

                var result = await _accountService.GetExternalAccountById(accountId, loggedInAccount);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Get external account failed");
            }
        }


        [Authorize]
        [HttpGet("getOwnAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOwnAccount()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized("You must to be logged in before you can access your own account");
                }

                var result = await _accountService.GetOwnAccount(loggedInAccount);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Get own account failed");
            }
        }


        /*
         * Consider Admin-role-priviledges to remove any account
         */

        [Authorize]
        [HttpDelete("deleteOwnAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOwnAccount()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized("You must to be logged in before you can delete your own account");
                }

                var result = await _accountService.DeleteOwnAccount(loggedInAccount);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Get own account failed");
            }
        }


        // Logout for JWT authentication is handled on the Client-side



    }
}
