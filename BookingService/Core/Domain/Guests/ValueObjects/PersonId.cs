using Domain.Guests.Enuns;

namespace Domain.Guests.ValueObjects
{
    public class PersonId
    {
        public string IdNumber { get; set; }
        public EDocumentType DocumentType { get; set; }
    }
}
