using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Completed
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly IKeywordService _keywordService;
        private readonly IAuthentication _authentication;

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
        public async Task<IActionResult> CreateKeyword([FromBody] KeywordCreateInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new { Message = "Invalid input.", Errors = errors });
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
            catch (Exception)
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
        public async Task<IActionResult> GetKeywordById(Guid keywordId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new { Message = "Invalid input.", Errors = errors });
                }

                var isUserLoggedIn = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(isUserLoggedIn))
                {
                    return Unauthorized("You must be logged in before you can get a keyword for one your profiles");
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
            catch (Exception)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Get profile failed");
            }
        }


        // UpdateKeyword
        [HttpPut("updateKeyword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateKeyword([FromBody] KeywordUpdateInput dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new { Message = "Invalid input.", Errors = errors });
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized("You must to be logged in before you can update your keyword");
                }

                var result = await _keywordService.UpdateKeyword(dto, loggedInAccount);

                if (result.isSucces)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                // TODO: log-info: AccountId (loggedInUser), UTC.Now, ex.stack-trace 
                return BadRequest("Updated profile failed");
            }
        }


        // DeleteKeywordById
        [HttpDelete("deleteKeyword/{keywordId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKeywordById(Guid keywordId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new { Message = "Invalid input.", Errors = errors });
                }

                var loggedInAccount = _authentication.GetCurrentUserId(User);

                if (string.IsNullOrEmpty(loggedInAccount))
                {
                    return Unauthorized($"You must to be logged in before you can delete keyword {keywordId} from your account");
                }

                var result = await _keywordService.DeleteKeywordById(keywordId, loggedInAccount);

                if (result.isSucces)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Delete keyword failed");
            }
        }


    }
}
