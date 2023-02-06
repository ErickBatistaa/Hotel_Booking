using Domain.Rooms.Ports;
using Domain.Rooms.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Rooms
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _hotelDbContext;

        public RoomRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<int> Create(Room room)
        {
            _hotelDbContext.Rooms.Add(room);
            await _hotelDbContext.SaveChangesAsync();
            return room.Id;
        }

        public async Task<Room> Get(int id)
        {
            return await _hotelDbContext.Rooms
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Room> GetAggregate(int id)
        {
            return await _hotelDbContext.Rooms
                .Include(b => b.Bookings)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> Update(Room room)
        {
            throw new NotImplementedException();
        }
    }
}
