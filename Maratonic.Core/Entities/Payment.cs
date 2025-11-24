using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // [Index] için gerekli

namespace Maratonic.Core.Entities
{
    [Table("Payments")]
    [Index(nameof(TransactionId), IsUnique = true)] // transaction_id varchar [unique]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // payment_id serial [pk]
        public int PaymentId { get; set; }

        // registration_id int [unique, not null, ref: > Registrations.registration_id]
        // [Unique] kısıtlamasını DbContext'te zorunlu kılacağız
        [Required]
        public int RegistrationId { get; set; }
        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public string TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string ProviderResponse { get; set; } // json tipindeki veri

        public bool IsSuccessful { get; set; } = false;
    }
}