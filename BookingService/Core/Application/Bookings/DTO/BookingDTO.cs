using Application.Guests.DTO;
using Application.Rooms.DTO;
using Domain.Bookings.Entities;
using Domain.Guests.Entities;
using Domain.Guests.Enuns;
using Domain.Rooms.Entities;

namespace Application.Bookings.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        private EStatus Status { get; set; }
        public int RoomId { get; set; } 
        public int GuestId { get; set; }

        public BookingDTO()
        {
            this.PlacedAt = DateTime.UtcNow;
        }

        public static Booking MapToEntity (BookingDTO bookingDTO)
        {
            return new Booking
            {
                Id = bookingDTO.Id,
                Start = bookingDTO.Start,
                End = bookingDTO.End,
                Guest = new Guest { id = bookingDTO.GuestId},
                Room = new Room { Id = bookingDTO.RoomId },
                PlacedAt = bookingDTO.PlacedAt
            };
        }

        public static BookingDTO MapToDTO(Booking booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                Start = booking.Start,
                End = booking.End,
                GuestId = booking.Guest.id,
                PlacedAt = booking.PlacedAt,
                RoomId = booking.Room.Id,
                Status = booking.Status
            };
        }
    }
}
