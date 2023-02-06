using Application.Bookings.Requests;
using Application.Bookings.Responses;
using MediatR;

namespace Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponse>
    {
        public CreateBookingRequest BookingDto { get; set; }
    }
}
