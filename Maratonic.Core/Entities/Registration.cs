using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maratonic.Core.Entities
{
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int RaceId { get; set; }
        public Race Race { get; set; }

        // 1-1 Payment
        public Payment? PaymentTransaction { get; set; }

        public int Status { get; set; }
    }
}
