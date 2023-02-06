using Application;
using Application.Bookings.Commands;
using Application.Bookings.DTO;
using Application.Bookings.DTO.Payments;
using Application.Bookings.Ports;
using Application.Bookings.Queries;
using Application.Bookings.Requests;
using Application.Bookings.Requests.Payments;
using Application.Bookings.Responses;
using Application.Payment.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingManager _bookingManager;
        private readonly ILogger<BookingController> _logger;
        private readonly IMediator _mediator;

        public BookingController(IBookingManager bookingManager, 
                                 ILogger<BookingController> logger,
                                 IMediator mediator)
        {
            _bookingManager = bookingManager;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Pay/{bookingId}")]
        public async Task<ActionResult<PaymentResponse>> Pay(PaymentBookingDTO paymentRequestDto,
                                                             int bookingId)
        {
            var request = new PaymentBookingRequest
            {
                Data = paymentRequestDto
            };

            request.Data.BookingId = bookingId;
            
            var result = await _bookingManager.PayForABooking(request);

            if (result.Success) return Ok(result.Data);

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Post(BookingDTO booking)
        {
            var request = new CreateBookingRequest
            {
                Data = booking
            };

            var command = new CreateBookingCommand
            {
                BookingDto = request
            };

            //var result = await _bookingManager.CreateBooking(request);

            var result = await _mediator.Send(command);

            if (result.Success) return Created("", result.Data);

            else if (result.ErrorCode == ErrorCodes.NOT_FOUND) return NotFound(result);

            else if (result.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION) return BadRequest(result);

            else if (result.ErrorCode == ErrorCodes.BOOKING_COULD_NOT_STORE_DATA) return BadRequest(result);

            _logger.LogError("Response with unknown ErrorCode returned", result);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<BookingResponse>> Get(int bookingId)
        {
            var query = new GetBookingQuery
            {
                Id = bookingId
            };

            var result = await _mediator.Send(query);

            if (result.Success) return Created("", result.Data);

            _logger.LogError("Could not process the request", result);
            return BadRequest(500);
        }
    }
}
