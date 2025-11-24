using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; } // Infrastructure'da Unique Index eklenecek

        [Required]
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigasyon Özellikleri
        public ICollection<Registration> Registrations { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Race> CreatedRaces { get; set; }
    }
}