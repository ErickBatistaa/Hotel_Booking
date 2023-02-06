using Domain.Guests.Entities;
using Domain.Guests.Enuns;
using Domain.Guests.ValueObjects;

namespace Application.Guests.DTO
{
    public class GuestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public int IdTypeCode { get; set; }

        public static Guest MapToEntity(GuestDTO guestDTO)
        {
            return new Guest
            {
                id = guestDTO.Id,
                Name = guestDTO.Name,
                Surname = guestDTO.Surname,
                Email = guestDTO.Email,
                Document = new PersonId
                {
                    IdNumber = guestDTO.IdNumber,
                    DocumentType = (EDocumentType)guestDTO.IdTypeCode
                }
            };
        }

        public static GuestDTO MapToDTO(Guest guest)
        {
            return new GuestDTO
            {
                Id = guest.id,
                Name = guest.Name,
                Surname = guest.Surname,
                Email = guest.Email,
                IdNumber = guest.Document.IdNumber,
                IdTypeCode = (int)guest.Document.DocumentType
            };
        }
    }
}
