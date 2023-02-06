using Application;
using Application.Rooms.Commands;
using Application.Rooms.DTO;
using Application.Rooms.Ports;
using Application.Rooms.Queries;
using Application.Rooms.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Room")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;
        private readonly IMediator _mediator;

        public RoomController(
            ILogger<RoomController> logger,
            IRoomManager roomManager,
            IMediator mediator)
        {
            _logger = logger;
            _roomManager = roomManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> Post(RoomDTO room)
        {
            var request = new CreateRoomRequest
            {
                Data = room,
                UserRoles = new List<string>() { "Manager", "AnotherUser" }
            };

            var command = new CreateRoomCommand
            {
                RoomDto = request
            };

            //var result = await _roomManager.CreateRoom(request);
            var result = await _mediator.Send(command);

            if (result != null) return Created("", result.Data);

            else if (result?.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION) return BadRequest(result);

            else if (result?.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA) return BadRequest(result);

            _logger.LogError("Response with unknown ErrorCode returned", result);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<RoomDTO>> Get(int roomId)
        {
            //var result = await _roomManager.GetRoom(roomId);

            var query = new GetRoomsQuery
            {
                Id = roomId
            };

            var result = await _mediator.Send(query);

            if (result.Success) return Created("", result.Data);

            return NotFound(result);
        }
    }
}
