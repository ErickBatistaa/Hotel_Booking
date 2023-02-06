using Domain.Guests.Ports;
using Domain.Guests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Guests
{
    public class GuestRepository : IGuestRepository
    {
        private HotelDbContext _hotelDbContext;

        public GuestRepository(HotelDbContext hotelDbContext) 
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<int> Create(Guest guest)  
        {
            _hotelDbContext.Guests.Add(guest);
            await _hotelDbContext.SaveChangesAsync();
            return guest.id;
        }

        public Task<Guest> Get(int id)
        {
            return _hotelDbContext.Guests.Where(g => g.id == id).FirstOrDefaultAsync();
        }
    }
}
