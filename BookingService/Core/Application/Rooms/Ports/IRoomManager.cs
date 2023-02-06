using Application.Rooms.Requests;
using Application.Rooms.Responses;

namespace Application.Rooms.Ports
{
    public interface IRoomManager
    {
        Task<RoomResponse> CreateRoom(CreateRoomRequest request);
        Task<RoomResponse> GetRoom(int roomId);
    }
}
