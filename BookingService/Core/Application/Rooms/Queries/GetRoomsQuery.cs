using Application.Rooms.Responses;
using MediatR;

namespace Application.Rooms.Queries
{
    public class GetRoomsQuery : IRequest<RoomResponse>
    {
        public int Id { get; set; }
    }
}
