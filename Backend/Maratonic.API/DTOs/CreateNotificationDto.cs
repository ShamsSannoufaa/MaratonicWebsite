namespace Maratonic.API.DTOs
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; } = "";
        public string Type { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
    }
}
