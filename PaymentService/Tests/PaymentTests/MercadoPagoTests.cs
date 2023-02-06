using Application;
using Application.Bookings.DTO.Payments;
using PaymentApplication;
using PaymentApplication.MercadoPago;

namespace PaymentTests
{
    public class MercadoPagoTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task Should_FailWhenPaymentIntentionStringIsInvalid()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var result = await provider.CapturePayment("");

            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.INVALID_PAYMENT_INTENTION);
            Assert.AreEqual(result.Message, "The selected payment intention is invalid");

        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var result = await provider.CapturePayment("https://mercadopago.com.br");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Message, "Payment successfully processed");
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.CreatedDate);
            Assert.NotNull(result.Data.PaymentId);
        }
            
    }
}
