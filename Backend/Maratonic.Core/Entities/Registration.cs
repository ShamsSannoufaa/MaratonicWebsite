using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maratonic.Core.Enums;

namespace Maratonic.Core.Entities
{
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // USER FK (Identity User → string Id)
        [Required]
        public string UserId { get; set; } = string.Empty;

        // RACE FK
        [Required]
        public int RaceId { get; set; }

        // RACE NAVIGATION
        public Race? Race { get; set; }

        // 💰 FEE (Eksik olan property — zorunlu)
        [Column(TypeName = "decimal(10,2)")]
        public decimal Fee { get; set; } = 0;

        // PAYMENT STATUS
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // PAYMENT 1 - 1 (nullable)
        public Payment? PaymentTransaction { get; set; }

        // DATE
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
