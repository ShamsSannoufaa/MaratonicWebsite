using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    // Bileşik Tekil Kısıtlama (user_id, race_id) AppDbContext'te tanımlanacaktır.
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // registration_id serial [pk]
        public int RegistrationId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int RaceId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending; // payment_status [not null]

        // Navigasyon Özellikleri:
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RaceId")]
        public Race Race { get; set; }

        // İlişkiler:
        public Payment PaymentTransaction { get; set; } // Bire Bir ilişki
    }
}