using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        // Foreign Keys
        [Required]
        public string UserId { get; set; }  // <-- DÜZELTİLDİ

        [Required]
        public int RaceId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        //[ForeignKey("UserId")]
        //public ApplicationUser User { get; set; }
        

        [ForeignKey("RaceId")]
        public Race Race { get; set; }

        public Payment PaymentTransaction { get; set; }
    }
}
