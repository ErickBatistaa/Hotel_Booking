using Application;
using Application.MercadoPago.Exceptions;
using Application.Payment.DTO;
using Application.Payment.Ports;
using Application.Payment.Responses;

namespace PaymentApplication.MercadoPago
{
    public class MercadoPagoAdapter : IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentIntention)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentIntention))
                    throw new InvalidPaymentIntentionException();

                paymentIntention += "/success";

                var paymentDTO = new PaymentStateDTO
                {
                    CreatedDate = DateTime.Now,
                    Message = $"Successfully paid {paymentIntention}",
                    PaymentId = "123",
                    Status = EStatus.Success
                };

                var response = new PaymentResponse()
                {
                    Data = paymentDTO,
                    Success = true,
                    Message = "Payment successfully processed"
                };

                return Task.FromResult(response);
            }

            catch (InvalidPaymentIntentionException)
            {
                var result = new PaymentResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_PAYMENT_INTENTION,
                    Message = "The selected payment intention is invalid"
                };

                return Task.FromResult(result);
            }
        }
    }
}
