namespace Application
{
    public enum ErrorCodes
    {
        // Guests related codes 1 to 99
        NOT_FOUND = 1,
        GUEST_COULD_NOT_STORE_DATA = 2,
        GUEST_INVALID_PERSON_ID = 3,
        GUEST_MISSING_REQUIRED_INFORMATION = 4,
        GUEST_INVALID_EMAIL = 5,
        GUEST_NOT_FOUND = 6,

        // Rooms related codes 100 to 199
        ROOM_NOT_FOUND = 100,
        ROOM_COULD_NOT_STORE_DATA = 101,
        ROOM_INVALID_PERSON_ID = 102,
        ROOM_MISSING_REQUIRED_INFORMATION = 103,
        ROOM_INVALID_EMAIL = 104,
        ROOM_INVALID_PERMISSION = 105,

        // Bookings related codes 200 to 299
        BOOKING_NOT_FOUND = 100,
        BOOKING_COULD_NOT_STORE_DATA = 101,
        BOOKING_INVALID_PERSON_ID = 102,
        BOOKING_MISSING_REQUIRED_INFORMATION = 103,
        BOOKING_INVALID_EMAIL = 104,
        BOOKING_ROOM_CANNOT_BE_BOOKED = 105,

        // Payment related codes 500 - 1500
        INVALID_PAYMENT_INTENTION = 500,
        PAYMENT_PROVIDER_NOT_IMPLEMENTED = 501
    }

    public abstract class Response // father class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
