using Domain.Guests.Enuns;

namespace Domain.Guests.ValueObjects
{
    public class Price
    {
        public decimal Value { get; set; }
        public EAcceptedCurrencies Currency { get; set; }
    }
}
