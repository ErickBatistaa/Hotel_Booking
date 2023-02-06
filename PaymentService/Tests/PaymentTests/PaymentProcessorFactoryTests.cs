using Application;
using Application.Bookings.DTO.Payments;
using PaymentApplication;
using PaymentApplication.MercadoPago;

namespace PaymentTests
{
    public class PaymentProcessorFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            Assert.AreEqual(provider.GetType(), typeof(NotImplementedPaymentProvider));
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            var result = await provider.CapturePayment("https://myprovider.com");

            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED);
            Assert.AreEqual(result.Message, "The selected payment provider is not available at the moment");
        }
    }
}