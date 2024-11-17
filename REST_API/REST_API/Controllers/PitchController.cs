using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_API.DTOs;
using REST_API.DTOs.ProfileDomain;
using REST_API.Services;
using REST_API.Util;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        private IPitchService _pitchService;

        public PitchController(IPitchService pitchService)
        {
            _pitchService = pitchService;
        }

        [Authorize]
        [HttpPost("sendPitch")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendPitch([FromBody] SendPitchDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _pitchService.SendPitch(dto);

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

    }
}
