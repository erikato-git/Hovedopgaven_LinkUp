using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var result = _accountService.Login(dto);

            // TDD ...


            return BadRequest(result.Message);          // default response
        }


        [HttpPost("createAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]             // JWT token
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CreateAccount([FromBody] CreateAccountDTO dto)
        {
            var result = _accountService.CreateAccount(dto);

            // TDD ...


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
            var result = _accountService.UpdateAccount(dto);

            // TDD ...


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
            var result = _accountService.GetAccountById(id);

            // TDD ...


            return BadRequest(result.Message);          // default response
        }


    }
}
