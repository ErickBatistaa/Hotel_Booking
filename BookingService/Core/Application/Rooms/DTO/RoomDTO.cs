using Domain.Guests.Enuns;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Entities;

namespace Application.Rooms.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public decimal Price { get; set; }
        public EAcceptedCurrencies Currency { get; set; }

        public static Room MapToEntity(RoomDTO roomDTO)
        {
            return new Room
            {
                Id = roomDTO.Id,
                Name = roomDTO.Name,
                Level = roomDTO.Level,
                Price = new Price { Currency = roomDTO.Currency, Value = roomDTO.Price },
                InMaintenance = roomDTO.InMaintenance
            };
        }

        public static RoomDTO MapToDTO(Room room)
        {
            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                Level = room.Level,
                Price = room.Price.Value,
                InMaintenance = room.InMaintenance
            };
        }
    }
}
