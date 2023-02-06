using Application.Rooms.DTO;
using Application.Rooms.Responses;
using Domain.Rooms.Ports;
using MediatR;

namespace Application.Rooms.Queries
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, RoomResponse>
    {
        private readonly IRoomRepository _roomRepository;

        public GetRoomsQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.Get(request.Id);

            if (room == null)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "Could not find a Room with the given Id"
                };
            }

            return new RoomResponse
            {
                Data = RoomDTO.MapToDTO(room),
                Success = true
            };
        }
    }
}
