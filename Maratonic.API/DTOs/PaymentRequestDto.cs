namespace Maratonic.API.DTOs
{
    public class PaymentRequestDto
    {
        public int RegistrationId { get; set; }
        public decimal Amount { get; set; }
        public string Provider { get; set; } = "Mock";
    }
}
