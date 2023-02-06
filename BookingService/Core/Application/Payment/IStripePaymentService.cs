using Application.Payment.DTO;

namespace Application.Payment
{
    public interface IStripePaymentService
    {
        Task<PaymentStateDTO> PayWithCreditCard(string paymentIntention);
        Task<PaymentStateDTO> PayWithDebitCard(string paymentIntention);
        Task<PaymentStateDTO> PayBankTransfer(string paymentIntention);
    }
}
