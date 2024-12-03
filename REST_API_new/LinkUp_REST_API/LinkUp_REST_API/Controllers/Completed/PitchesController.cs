using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Services.Interfaces.Completed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchesController : ControllerBase
    {
        private IPitchService _pitchService;
        private IAuthentication _authentication;

        public PitchesController(IPitchService pitchService, IAuthentication authentication)
        {
            _pitchService = pitchService;
            _authentication = authentication;
        }


        // CreatePitch
        [HttpPost("createPitch")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePitch([FromBody] PitchCreateInput dto)
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
                    return Unauthorized("You must be logged in before you can send a pitch");
                }

                var result = await _pitchService.CreatePitch(dto, isUserLoggedIn);

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
                return BadRequest("Create pitch failed");
            }
        }


        // GetAllAssociatedPitches
        [HttpGet("getAllAssociatedPitches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllAssociatedPitches()
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
                    return Unauthorized("You must be logged in before you can get associated pitches for your account");
                }

                var result = await _pitchService.GetAllAssociatedPithes(isUserLoggedIn);

                if (result.isSucces)
                {
                    // TODO: insert 'GetByAccountId' path in ""
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
                return BadRequest("Create pitch failed");
            }
        }


        // GetPitchById
        [HttpGet("getPitchById/{pitchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPitchById([FromQuery] Guid pitchId)
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
                    return Unauthorized("You must be logged in before you can get associated pitches for your account");
                }

                var result = await _pitchService.GetPitchById(pitchId, isUserLoggedIn);

                if (result.isSucces)
                {
                    // TODO: insert 'GetByAccountId' path in ""
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
                return BadRequest("Get pitch failed");
            }
        }


        // DeletePitchById: restricted to sendingProfile pitches
        [HttpDelete("deletePitch/{pitchId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePitchById([FromQuery] Guid pitchId)
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
                    return Unauthorized("You must be logged in before you can delete sended pitches");
                }

                var result = await _pitchService.DeletePitchById(pitchId, isUserLoggedIn);

                if (result.isSucces)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., _logger.LogError(ex, "Login failed"))
                return BadRequest("Delete pitch failed");
            }
        }



        // UpdatePitch (omitted): when a pitch has been send it's shouldn't be possible to edit the message like an email



    }
}
