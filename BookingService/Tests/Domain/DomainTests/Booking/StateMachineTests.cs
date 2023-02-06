using Domain.Bookings.Entities;
using Domain.Guests.Enuns;

namespace DomainTests.Bookings
{
    public class StateMachineTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();

            Assert.AreEqual(booking.Status, EStatus.Created);
        }

        [Test] 
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus() 
        {
            var booking = new Booking();
            
            booking.ChangeState(EAction.Pay);

            Assert.AreEqual(booking.Status, EStatus.Paid);
        }

        [Test]
        public void ShouldSetStatusToCanceledWhenCancelingABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Cancel);

            Assert.AreEqual(booking.Status, EStatus.Canceled);
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingAPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Pay);
            booking.ChangeState(EAction.Finish);

            Assert.AreEqual(booking.Status, EStatus.Finished);
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingAPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Pay);
            booking.ChangeState(EAction.Refound);

            Assert.AreEqual(booking.Status, EStatus.Refound);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Cancel);
            booking.ChangeState(EAction.Reopen);

            Assert.AreEqual(booking.Status, EStatus.Created);
        }

        [Test]
        public void ShouldNotChangeStatusWhenRefoundingACreatedABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Refound);

            Assert.AreEqual(booking.Status, EStatus.Created);
        }

        [Test]
        public void ShouldNotChangeStatusWhenRefoundingAFinishedBooking()
        {
            var booking = new Booking();

            booking.ChangeState(EAction.Pay);
            booking.ChangeState(EAction.Finish);
            booking.ChangeState(EAction.Refound);

            Assert.AreEqual(booking.Status, EStatus.Finished);
        }
    }
}
