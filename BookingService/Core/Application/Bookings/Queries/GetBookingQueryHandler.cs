using Application.Bookings.DTO;
using Application.Bookings.Responses;
using Domain.Bookings.Ports;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var bookingDto = BookingDTO.MapToDTO(await _bookingRepository.GetBooking(request.Id));

            return new BookingResponse
            {
                Success = true,
                Data = bookingDto
            };
        }
    }
}
