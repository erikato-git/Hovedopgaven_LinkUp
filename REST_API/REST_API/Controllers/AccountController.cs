using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Services;
using REST_API.Util;
using System.ComponentModel.DataAnnotations;

namespace REST_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]             // JWT token
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody]LoginDTO dto)
        {
            /*
             * ModelState: https://code-maze.com/aspnetcore-modelstate-validation-web-api/
             */
            if (!ModelState.IsValid)
            {
                return Unauthorized(ModelState);
            }

            var result = _accountService.Login(dto);

            /*
             * login in ServiceAccount doesn't return a nullable, just in case future programmers set it to nullable the AccountController won't break
             * TODO: Consider to wrap the implementation in try-catch-blocks like in the service-classes and repository-classes
             */
            if (result == null)
            {
                return BadRequest();
            }

            // Succes 

            if (result.isSuccess)
            {
                return Ok("JWT token");
            }

            // Errors

            // more checks ...

            return BadRequest(result.Message);          // default response
        }


        [HttpPost("createAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]             // JWT token
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CreateAccount([FromBody] CreateAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _accountService.CreateAccount(dto);

            if(result == null)
            {
                return BadRequest();
            }

            // Succes

            if(result.isSuccess)
            {
                return Created();
            }

            // Errors

            if(result.Message.Equals(ErrorMessages.AccountService_CreateAccount_409InvalidEmail))
            {
                return Conflict(result.Message);
            }

            // more checks ...


            return BadRequest(result.Message);          // default response
        }


        [HttpPut("updateAccount")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]            
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult UpdateAccount([FromBody] UpdateAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _accountService.UpdateAccount(dto);

            if (result == null)
            {
                return BadRequest();
            }

            // Sucess

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            // Errors

            if(result.Message.Equals(ErrorMessages.AccountService_UpdateAccount_403CannotUpdateAnotherAccount))
            {
                return Forbid();
            }

            if (result.Message.Equals(ErrorMessages.AccountService_UpdateAccount_409UserChangeEmailToAnotherEmailThatAlreadyExist))
            {
                return Conflict(result.Message);
            }

            // more checks ...

            return BadRequest(result.Message);          // default response
        }


        [HttpGet("getAccount/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAccountById([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _accountService.GetAccountById(id);

            if (result == null)
            {
                return BadRequest();
            }

            // Success

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            // Errors

            if (result.Message.Equals(ErrorMessages.AccountService_GetAccountById_403UserTriesToAccessAnotherAccount))
            {
                return Forbid();
            }

            // more checks ...


            return BadRequest(result.Message);          // default response
        }



        // TODO: DeleteAccountById
        // make specifications first

    }
}
