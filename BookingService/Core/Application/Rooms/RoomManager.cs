using Application.Rooms.DTO;
using Application.Rooms.Ports;
using Application.Rooms.Requests;
using Application.Rooms.Responses;
using Domain.Rooms.Exceptions;
using Domain.Rooms.Ports;

namespace Application.Rooms
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _roomRepository;

        public RoomManager(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                if(!request.UserRoles.Contains("Manager"))
                {
                    return new RoomResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.ROOM_INVALID_PERMISSION,
                        Message = "User does not have permission to perform this action"
                    };
                }

                var room = RoomDTO.MapToEntity(request.Data);
                await room.Save(_roomRepository);
                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Success = true,
                    Data = request.Data
                };
            }
            catch (InvalidRoomDataException)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch(Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<RoomResponse> GetRoom(int roomId)
        {
            var room = await _roomRepository.Get(roomId);

            if (room == null)
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "No room record was found with the given ID"
                };

            return new RoomResponse { Success = true, Data = RoomDTO.MapToDTO(room) };
        }
    }
}
