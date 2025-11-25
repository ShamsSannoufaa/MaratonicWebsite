using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        // Foreign Key
        [Required]
        public int RegistrationId { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public string TransactionId { get; set; } // Unique index Infrastructure’da oluşturulacak

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string ProviderResponse { get; set; }

        public bool IsSuccessful { get; set; } = false;
    }
}
