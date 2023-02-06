using Application.Guests.DTO;

namespace Application.Guests.Responses
{
    public class GuestResponse : Response // children class
    {
        public GuestDTO Data { get; set; }
    }
}
