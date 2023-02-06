using Domain.Guests.Entities;

namespace Domain.Guests.Ports
{
    public interface IGuestRepository
    {
        Task<Guest> Get(int id);
        Task<int> Create(Guest guest);
    }
}
