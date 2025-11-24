using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    // Bileşik Tekil Kısıtlama Infrastructure'da tanımlanacak.
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        // Foreign Keys
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RaceId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Navigasyon Özellikleri
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RaceId")]
        public Race Race { get; set; }

        public Payment PaymentTransaction { get; set; } // Bire Bir ilişki
    }
}