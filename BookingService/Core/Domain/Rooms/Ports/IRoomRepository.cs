using Domain.Rooms.Entities;

namespace Domain.Rooms.Ports
{
    public interface IRoomRepository 
    {
        Task<int> Create(Room room);
        Task<int> Update(Room room);
        Task<Room> Get(int id);
        Task<Room> GetAggregate(int id);
    }
}
