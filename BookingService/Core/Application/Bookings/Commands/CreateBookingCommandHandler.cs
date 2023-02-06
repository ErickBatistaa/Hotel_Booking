using Application.Bookings.DTO;
using Application.Bookings.Responses;
using Domain.Bookings.Exceptions;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Exceptions;
using Domain.Rooms.Ports;
using MediatR;

namespace Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, 
                                           IRoomRepository roomRepository,
                                           IGuestRepository guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
        }

        public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = BookingDTO.MapToEntity(request.BookingDto.Data);

                booking.Guest = await _guestRepository.Get(request.BookingDto.Data.GuestId);
                booking.Room = await _roomRepository.GetAggregate(request.BookingDto.Data.RoomId);

                await booking.Save(_bookingRepository);

                request.BookingDto.Data.Id = booking.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = request.BookingDto.Data
                };
            }

            catch (BookingRequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required booking information passed"
                };
            }

            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "The selected Room is not available"
                };
            }

            catch (Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
