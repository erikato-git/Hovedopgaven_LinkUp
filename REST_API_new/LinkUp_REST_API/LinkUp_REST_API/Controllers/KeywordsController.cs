using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private IKeywordService _keywordService;
        private IAuthentication _authentication;

        public KeywordsController(IKeywordService keywordService, IAuthentication authentication)
        {
            _keywordService = keywordService;
            _authentication = authentication;
        }

        // CreateKeyword
        [HttpPost("createKeyword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateKeyword([FromBody] KeywordCreateUpdateInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isUserLoggedIn = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(isUserLoggedIn))
                {
                    return Unauthorized("You must be logged in before you can create a keyword for your profile");
                }

                var result = await _keywordService.CreateKeyword(dto, isUserLoggedIn);

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
                return BadRequest("Create keyword failed");
            }
        }

        // GetKeywordById
        [HttpGet("getKeyword/{keywordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetKeywordById([FromQuery] Guid keywordId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isUserLoggedIn = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(isUserLoggedIn))
                {
                    return Unauthorized("You must be logged in before you can create a profile for your account");
                }

                var result = await _keywordService.GetKeywordById(keywordId, isUserLoggedIn);

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
                return BadRequest("Get profile failed");
            }
        }


        // UpdateKeyword


        // DeleteKeywordById
    }
}
