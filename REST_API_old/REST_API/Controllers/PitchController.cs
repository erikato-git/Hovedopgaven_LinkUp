using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_API.Controllers.IHelpers;
using REST_API.DTOs.PitchDomain;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        private IPitchService _pitchService;
        private IPitchControllerHelper _pitchControllerHelper;

        public PitchController(IPitchService pitchService, IPitchControllerHelper pitchControllerHelper)
        {
            _pitchService = pitchService;
            _pitchControllerHelper = pitchControllerHelper;
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

            var userAccountId = _pitchControllerHelper.ExtractUserAccountId(User);

            var result = await _pitchService.SendPitch(dto, userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Created("", result.Data);
            }

            if (result.Message.Equals(ErrorMessages.PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError))
            {
                return new ObjectResult(new { })
                {
                    Value = result.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            if (result.Message.Equals(ErrorMessages.PitchService_SendPitch_ReceipientsAccountDoesNotExist))
            {
                return NotFound(result.Message);
            }

            if (result.Message.Equals(ErrorMessages.PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile))
            {
                return Conflict(result.Message);
            }


            return BadRequest(result.Message);
        }


        [Authorize]
        [HttpPost("getIncomingPitches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIncomingPitches()
        {
            var userAccountId = _pitchControllerHelper.ExtractUserAccountId(User);

            var result = await _pitchService.GetIncomingPitches(userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError))
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
        [HttpPost("getOutcomingPitches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOutcomingPitches()
        {
            var userAccountId = _pitchControllerHelper.ExtractUserAccountId(User);

            var result = await _pitchService.GetOutcomingPitches(userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            if (result.isSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Message.Equals(ErrorMessages.PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError))
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
