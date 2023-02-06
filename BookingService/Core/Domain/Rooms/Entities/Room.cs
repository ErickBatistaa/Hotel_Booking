using Domain.Bookings.Entities;
using Domain.Guests.Enuns;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Exceptions;
using Domain.Rooms.Ports;

namespace Domain.Rooms.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public ICollection<Booking>? Bookings { get; set; }

        public bool IsAvailable
        {
            get
            {
                if (InMaintenance || HasGuest) return false;
                return true;
            }
        }
        public bool HasGuest
        {
            get
            {
                var notAvailableStatuses = new List<EStatus>()
                {
                    EStatus.Created,
                    EStatus.Paid
                };

                return this.Bookings?.Where(
                    b => b.Room.Id == this.Id && 
                    notAvailableStatuses.Contains(b.Status)).Count() > 0;
            }
        }

        public bool CanBeBooked()
        {
            try
            {
                this.ValidateState();
            }
            catch (Exception)
            {
                return false;
            }

            if (!this.IsAvailable)
                return false;

            return true;
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(this.Name) ||
                this.Price == null ||
                this.Price.Value < 10) 
                throw new InvalidRoomDataException();
        }

        public async Task Save (IRoomRepository roomRepository)
        {
            this.ValidateState();

            if (this.Id == 0) this.Id = await roomRepository.Create(this);

            else await roomRepository.Update(this);
        }
    }
}
