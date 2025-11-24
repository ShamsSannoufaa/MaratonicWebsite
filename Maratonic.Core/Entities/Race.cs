using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Races")]
    public class Race
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RegistrationFee { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }

        public RaceStatus Status { get; set; } = RaceStatus.Upcoming;

        // Foreign Key
        public int CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigasyon Özellikleri
        public ICollection<Registration> Registrations { get; set; }
    }
}