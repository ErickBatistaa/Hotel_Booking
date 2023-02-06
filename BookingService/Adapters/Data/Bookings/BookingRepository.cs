using Domain.Bookings.Entities;
using Domain.Bookings.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _hotelDbContext;

        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            _hotelDbContext.Bookings.Add(booking);
            await _hotelDbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Booking> GetBooking(int bookingId)
        {
            return _hotelDbContext.Bookings
                .Include(g => g.Guest)
                .Include(r => r.Room)
                .Where(b => b.Id == bookingId).FirstOrDefaultAsync();
        }
    }
}
