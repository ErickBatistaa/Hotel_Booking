using Application.Bookings.Responses;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GetBookingQuery : IRequest<BookingResponse>
    {
        public int Id { get; set; }
    }
}
