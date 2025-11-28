namespace Maratonic.API.DTOs
{
    public class RegistrationWithPaymentResponseDto
    {
        public int RegistrationId { get; set; }
        public string UserId { get; set; }

        public RaceDto Race { get; set; }
        public PaymentDto Payment { get; set; }

        public DateTime RegistrationDate { get; set; }
    }

    public class RaceDto
    {
        public int RaceId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RefundedAt { get; set; }
    }
}
