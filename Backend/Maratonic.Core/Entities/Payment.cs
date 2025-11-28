using Maratonic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RegistrationId { get; set; }
        public Registration Registration { get; set; }

        public decimal Amount { get; set; }

        public string Provider { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string ProviderResponse { get; set; } = string.Empty;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RefundedAt { get; set; }
    }
}
