namespace Maratonic.API.DTOs
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
