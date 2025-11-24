using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Races")]
    public class Race
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // race_id serial [pk]
        public int RaceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // registration_fee decimal [not null]
        public decimal RegistrationFee { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }

        public RaceStatus Status { get; set; } = RaceStatus.Upcoming;

        // Foreign Key: created_by int [ref: > Users.user_id]
        public int CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public User CreatedBy { get; set; } // Navigasyon Özelliği

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişkiler:
        public ICollection<Registration> Registrations { get; set; }
    }
}