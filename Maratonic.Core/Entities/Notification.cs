using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        // Foreign Key (Identity User → string Id)
        [Required]
        public string UserId { get; set; }  // <-- int → string yapıldı

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string Type { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public DateTime? SentAt { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
