using Microsoft.AspNetCore.Identity;

namespace Maratonic.Core.Entities
{
    // IdentityUser -> AspNetUsers tablosunun temel kolonlarını sağlar:
    // Id, UserName, NormalizedUserName,
    // Email, PasswordHash, PhoneNumber, SecurityStamp vb.
    public class ApplicationUser : IdentityUser
    {
        // Ek alanlar (DB'ye eklenecek)
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İstersen aşağıdaki navigasyon özelliklerini açabiliriz.
        // Şu anda gerekli değiller, kapalı kalmaları doğru.
        //
        // public ICollection<Registration> Registrations { get; set; }
        // public ICollection<Notification> Notifications { get; set; }
        // public ICollection<Race> CreatedRaces { get; set; }
    }
}
