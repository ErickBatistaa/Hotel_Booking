using Application.Bookings;
using Application.Bookings.DTO.Payments;
using Application.Bookings.Requests.Payments;
using Application.Payment.DTO;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Ports;
using Moq;

namespace ApplicationTests
{
    public class BookingManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Should_PayForABooking()
        {
            var dto = new PaymentBookingDTO
            {
                SelectedPaymentProvider = SupportedPaymentProviders.MercadoPago,
                PaymentIntention = "https://www.mercadopago.com.br",
                SelectedPaymentMethod = SupportedPaymentMethods.CreditCard
            };

            var paymentRequest = new PaymentBookingRequest
            {
                Data = dto
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDTO
            {
                CreatedDate = DateTime.Now,
                Message = $"Successfully paid {dto.PaymentIntention}",
                PaymentId = "123",
                Status = EStatus.Success
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Success = true,
                Message = "Payment successfully processed"
            };

            paymentProcessor.Setup(x => x.CapturePayment(dto.PaymentIntention))
                            .Returns(Task.FromResult(response));

            paymentProcessorFactory.Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider))
                                   .Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                                            bookingRepository.Object,
                                            guestRepository.Object,
                                            roomRepository.Object,
                                            paymentProcessorFactory.Object);

            var result = await bookingManager.PayForABooking(paymentRequest);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.AreEqual(result.Message, "Payment successfully processed");
        }
    }
}
