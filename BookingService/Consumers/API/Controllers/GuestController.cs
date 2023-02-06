using Application;
using Application.Guests.DTO;
using Application.Guests.Ports;
using Application.Guests.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Guest")]
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestController(
            ILogger<GuestController> logger, 
            IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest
            };

            var result = await _guestManager.CreateGuest(request);

            if (result.Success) return Created("", result.Data);

            else if (result.ErrorCode == ErrorCodes.NOT_FOUND) return NotFound(result);

            else if (result.ErrorCode == ErrorCodes.GUEST_INVALID_PERSON_ID) return BadRequest(result);

            else if (result.ErrorCode == ErrorCodes.GUEST_MISSING_REQUIRED_INFORMATION) return BadRequest(result);

            else if (result.ErrorCode == ErrorCodes.GUEST_INVALID_EMAIL) return BadRequest(result);

            else if (result.ErrorCode == ErrorCodes.GUEST_COULD_NOT_STORE_DATA) return BadRequest(result);

            _logger.LogError("Response with unknown ErrorCode returned", result);
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int guestId)
        {
            var result = await _guestManager.GetGuest(guestId);

            if (result.Success) return Created("", result.Data);

            return NotFound(result);
        }
    }
}
