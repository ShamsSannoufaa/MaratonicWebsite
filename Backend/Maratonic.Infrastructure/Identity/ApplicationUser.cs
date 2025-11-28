using Microsoft.AspNetCore.Identity;

namespace Maratonic.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gender { get; set; } = "";
        public int BirthYear { get; set; }
        public string Club { get; set; } = "";
        public string EmergencyContact { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
