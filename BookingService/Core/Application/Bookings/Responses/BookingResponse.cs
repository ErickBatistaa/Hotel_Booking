using Application.Bookings.DTO;

namespace Application.Bookings.Responses
{
    public class BookingResponse : Response
    {
        public BookingDTO Data { get; set; }
    }
}
