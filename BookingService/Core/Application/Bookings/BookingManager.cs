using Application.Bookings.DTO;
using Application.Bookings.Ports;
using Application.Bookings.Requests;
using Application.Bookings.Requests.Payments;
using Application.Bookings.Responses;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.Bookings.Exceptions;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Exceptions;
using Domain.Rooms.Ports;

namespace Application.Bookings
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;


        public BookingManager(IBookingRepository bookingRepository,
                              IGuestRepository guestRepository,
                              IRoomRepository roomRepository,
                              IPaymentProcessorFactory paymentProcessorFactory)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<BookingResponse> CreateBooking(CreateBookingRequest request)
        {
            try
            {
                var booking = BookingDTO.MapToEntity(request.Data);

                booking.Guest = await _guestRepository.Get(request.Data.GuestId);
                booking.Room = await _roomRepository.GetAggregate(request.Data.RoomId);

                await booking.Save(_bookingRepository); 

                request.Data.Id = booking.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = request.Data
                };
            }

            catch (BookingRequiredInformationException)
            {
                return new BookingResponse 
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required booking information passed"
                };
            }

            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "The selected Room is not available"
                };
            }

            catch (Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public Task<BookingDTO> GetBooking(int bookingId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentResponse> PayForABooking(PaymentBookingRequest request)
        {
            var paymentProcessor = _paymentProcessorFactory.GetPaymentProcessor(request.Data.SelectedPaymentProvider);

            var response = await paymentProcessor.CapturePayment(request.Data.PaymentIntention);

            if (response.Success)
            {
                return new PaymentResponse
                {
                    Success = true,
                    Data = response.Data,
                    Message = "Payment successfully processed"
                };
            }

            return response;
        }
    }
}
