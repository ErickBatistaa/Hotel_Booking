using Domain.Bookings.Exceptions;
using Domain.Bookings.Ports;
using Domain.Guests.Entities;
using Domain.Guests.Enuns;
using Domain.Rooms.Entities;
using Domain.Rooms.Exceptions;

namespace Domain.Bookings.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public EStatus Status { get; set; }
        public Room Room { get; set; } // Foreign Key to Rooms table
        public Guest Guest { get; set; } // Foreign Key to Guests table

        public Booking()
        {
            Status = EStatus.Created;
        }

        public void ChangeState(EAction action) // state machine (design pattern by state pattern)
        {
            Status = (Status, action) switch
            {
                (EStatus.Created, EAction.Pay) => EStatus.Paid,
                (EStatus.Created, EAction.Cancel) => EStatus.Canceled,
                (EStatus.Paid, EAction.Finish) => EStatus.Finished,
                (EStatus.Paid, EAction.Refound) => EStatus.Refound,
                (EStatus.Canceled, EAction.Reopen) => EStatus.Created,
                _ => Status
            };
        }

        public bool IsValid()
        {
            try
            {
                this.ValidateState();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ValidateState()
        {
            if (this.PlacedAt == default(DateTime) ||
                this.Start == default(DateTime) || 
                this.End == default(DateTime) || 
                this.Guest == null || 
                this.Room == null) 
                    throw new BookingRequiredInformationException();
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.ValidateState();

            this.Guest.IsValid();

            if (!this.Room.CanBeBooked())
                throw new RoomCannotBeBookedException();

            if (this.Id == 0)
            {
               var booking = await bookingRepository.CreateBooking(this);
                this.Id = booking.Id;
            } 
        }

    }
}
