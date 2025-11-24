using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // [Index] için kullanacağız

namespace Maratonic.Core.Entities
{
    [Table("Users")]
    [Index(nameof(Email), IsUnique = true)] // email varchar [unique] kısıtlaması
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // user_id serial [pk]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRole Role { get; set; } // user_role [not null]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişkiler: (1-Çok)
        public ICollection<Registration> Registrations { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Race> CreatedRaces { get; set; } // Yönetici'nin oluşturduğu yarışlar
    }
}