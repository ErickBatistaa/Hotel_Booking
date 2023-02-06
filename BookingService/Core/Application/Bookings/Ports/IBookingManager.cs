using Application.Bookings.DTO;
using Application.Bookings.Requests;
using Application.Bookings.Requests.Payments;
using Application.Bookings.Responses;
using Application.Payment.Responses;

namespace Application.Bookings.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(CreateBookingRequest request);
        Task<BookingDTO> GetBooking(int bookingId);
        Task<PaymentResponse> PayForABooking(PaymentBookingRequest request);

    }
}
