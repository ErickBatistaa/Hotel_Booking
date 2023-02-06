using Application.Rooms.DTO;
using Application.Rooms.Responses;
using Domain.Rooms.Exceptions;
using Domain.Rooms.Ports;
using MediatR;

namespace Application.Rooms.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
    {
        private IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = RoomDTO.MapToEntity(request.RoomDto.Data);

                await room.Save(_roomRepository);

                request.RoomDto.Data.Id = room.Id;

                return new RoomResponse
                {
                    Success = true,
                    Data = request.RoomDto.Data
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
            catch (Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
