using Domain.Bookings.Entities;

namespace Domain.Bookings.Ports
{
    public interface IBookingRepository
    {
        Task<Booking> GetBooking(int bookingId);
        Task<Booking> CreateBooking(Booking booking);
    }
}
