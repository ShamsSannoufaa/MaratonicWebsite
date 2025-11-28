namespace Maratonic.API.DTOs
{
    public class RegistrationResponseDto
    {
        public int RegistrationId { get; set; }
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public decimal Fee { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
