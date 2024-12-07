using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Completed
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IAuthentication _authentication;

        public ProfilesController(IProfileService profileService, IAuthentication authentication)
        {
            _profileService = profileService;
            _authentication = authentication;
        }

        //CreateProfile
        [HttpPost("createProfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProfile([FromBody] ProfileCreateInput dto)
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
                    return Unauthorized("You must be logged in before you can create a profile for your account");
                }

                var result = await _profileService.CreateProfile(dto, isUserLoggedIn);

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
                return BadRequest("Create profile failed");
            }
        }


        //GetProfileById
        [HttpGet("getProfile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfileById(Guid profileId)
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
                    return Unauthorized("You must be logged in before you can get a profile for your account");
                }

                var result = await _profileService.GetProfileById(profileId);

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

        //UpdateProfile
        [HttpPut("updateProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateInput dto)
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
                    return Unauthorized("You must to be logged in before you can update your profile");
                }

                var result = await _profileService.UpdateProfile(dto, loggedInAccount);

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


        //DeleteProfileById
        [HttpDelete("deleteProfile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProfileById(Guid profileId)
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
                    return Unauthorized("You must to be logged in before you can delete one of your own profiles");
                }

                var result = await _profileService.DeleteProfileById(profileId, loggedInAccount);

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
                return BadRequest("Delete profile failed");
            }
        }


        // SearchProfiles
        [HttpGet("searchProfiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchQuery([FromQuery] ProfileSearchQueryInput query)
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
                    return Unauthorized("You must to be logged in before you can search for profiles");
                }

                var result = await _profileService.SearchQuery(query);

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
                return BadRequest("Search profiles failed");
            }
        }

        //AddToFavorites
        [HttpPost("addToFavorites/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToFavorites(Guid profileId)
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
                    return Unauthorized("You must to be logged in before you can add a profile to favorites");
                }

                var result = await _profileService.AddProfileToFavorites(profileId, loggedInAccount);

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
                return BadRequest("Add profile to favorites failed");
            }
        }


        //RemoveFromFavorites
        [HttpDelete("removeFromFavorites/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFromFavorites(Guid profileId)
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
                    return Unauthorized("You must to be logged in before you can remove a profile from favorites");
                }

                var result = await _profileService.RemoveProfileFromFavorites(profileId, loggedInAccount);

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
                return BadRequest("Add profile to favorites failed");
            }
        }



    }
}
