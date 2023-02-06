namespace Application.Payment.DTO
{
    public enum EStatus
    {
        Success = 0,
        Failed = 1,
        Error = 2,
        Underfined = 3
    }

    public class PaymentStateDTO
    {
        public EStatus Status { get; set; }
        public string PaymentId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Message { get; set; }
    }
}
