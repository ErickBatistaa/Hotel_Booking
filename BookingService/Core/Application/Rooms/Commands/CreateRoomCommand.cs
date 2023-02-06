using Application.Rooms.Requests;
using Application.Rooms.Responses;
using MediatR;

namespace Application.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<RoomResponse>
    {
        public CreateRoomRequest RoomDto { get; set; }
    }
}
