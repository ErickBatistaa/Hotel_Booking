using Application.Rooms.DTO;

namespace Application.Rooms.Requests
{
    public class CreateRoomRequest
    {
        public RoomDTO Data { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
