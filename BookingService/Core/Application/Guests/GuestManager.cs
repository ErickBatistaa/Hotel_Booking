using Application.Guests.DTO;
using Application.Guests.Ports;
using Application.Guests.Requests;
using Application.Guests.Responses;
using Domain.Guests.Exceptions;
using Domain.Guests.Ports;

namespace Application.Guests
{
    public class GuestManager : IGuestManager
    {
        private readonly IGuestRepository _guestRepository;

        public GuestManager(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDTO.MapToEntity(request.Data);

                await guest.Save(_guestRepository);

                request.Data.Id = guest.id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Success = true
                };
            }

            catch (InvalidPersonDocumentIdException)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_INVALID_PERSON_ID,
                    Message = "The ID passed is not valid"
                };
            }

            catch (MissingRequiredInformation)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing Required Information passed"
                };
            }

            catch (InvalidEmailException)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }

            catch (Exception)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<GuestResponse> GetGuest(int guestId)
        {
            var guest = await _guestRepository.Get(guestId);

            if (guest == null)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_NOT_FOUND,
                    Message = "No guest record was found with the given ID"
                };
            }

            return new GuestResponse
            {
                Data = GuestDTO.MapToDTO(guest),
                Success = true
            };
        }
    }
}
