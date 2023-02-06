using Application.Bookings.DTO.Payments;

namespace Application.Payment.Ports
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider);
    }
}
