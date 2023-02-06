using Domain.Guests.Exceptions;
using Domain.Guests.Ports;
using Domain.Guests.ValueObjects;

namespace Domain.Guests.Entities
{
    public class Guest
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public PersonId Document { get; set; }

        public bool IsValid()
        {
            this.ValidateState();
            return true;    
        }

        private void ValidateState()
        {
            if (Document == null ||
                string.IsNullOrEmpty(Document.IdNumber) ||
                Document.IdNumber.Length <= 3 ||
                Document.DocumentType == 0)
            {
                throw new InvalidPersonDocumentIdException();
            }

            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Surname))
            {
                throw new MissingRequiredInformation();
            }

            else if (string.IsNullOrEmpty(Email) || Utils.ValidateEmail(this.Email) == false)
            {
                throw new InvalidEmailException();
            }
        }

        public async Task Save(IGuestRepository guestRepository)
        {
            this.ValidateState();

            if (this.id == 0)
            {
                this.id = await guestRepository.Create(this);
            }

            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
