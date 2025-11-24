using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // notification_id serial [pk]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? SentAt { get; set; } // nullable timestamp
        public bool IsSent { get; set; } = false;
    }
}