using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_API.Controllers.IHelpers;
using REST_API.DTOs.ProfileDomain;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfileService _profileService;
        private IProfileControllerHelper _profileControllerHelper;

        public ProfileController(IProfileService profileService, IProfileControllerHelper profileControllerHelper)
        {
            _profileService = profileService;
            _profileControllerHelper = profileControllerHelper;
        }

        [Authorize]
        [HttpPost("createProfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _profileControllerHelper.ExtractUserAccountId(User);

            var result = await _profileService.CreateProfile(dto, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Created("", result.Data);
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError))
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
        [HttpPut("updateProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _profileControllerHelper.ExtractUserAccountId(User);

            var result = await _profileService.UpdateProfile(dto, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_UpdateProfile_FailedToUpdateProfileDueToInternalServerError))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount))
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
        [HttpDelete("deleteProfile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProfile([FromQuery] Guid profileId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _profileControllerHelper.ExtractUserAccountId(User);

            var result = await _profileService.DeleteProfile(profileId, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return NoContent();
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError))
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
        [HttpGet("getProfile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfileById([FromQuery] Guid profileId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _profileControllerHelper.ExtractUserAccountId(User);

            var result = await _profileService.GetProfileById(profileId, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId))
            {
                return NotFound(result.Message);
            }


            return BadRequest(result.Message);
        }


        [Authorize]
        [HttpGet("searchProfiles/{query}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProfile([FromQuery] SearchQueryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _profileService.SearchQuery(dto);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError))
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
        [HttpGet("saveProfile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveProfile([FromQuery] Guid profileId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAccountId = _profileControllerHelper.ExtractUserAccountId(User);

            var result = await _profileService.SaveProfile(profileId, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.PitchService_SaveProfile_ProfileFailedToBeAddedToAccountsListForSavedProfilesDueToInternalServerError))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            //if (result.Message.Equals(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId))
            //{
            //    return NotFound(result.Message);
            //}


            return BadRequest(result.Message);
        }



    }
}
