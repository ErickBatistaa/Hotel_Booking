using Application.Bookings.DTO.Payments;
using Application.Payment.Ports;
using PaymentApplication.MercadoPago;

namespace PaymentApplication
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider)
        {
            switch (selectedPaymentProvider)
            {
                case SupportedPaymentProviders.MercadoPago:
                    return new MercadoPagoAdapter();

                default:
                    return new NotImplementedPaymentProvider();
            }
        }
    }
}
