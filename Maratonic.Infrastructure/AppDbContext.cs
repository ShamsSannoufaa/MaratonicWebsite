using Microsoft.EntityFrameworkCore;
using Maratonic.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Identity için kritik

namespace Maratonic.Infrastructure
{
    // YENİ HALİ: IdentityDbContext'ten miras alarak AspNetUsers, AspNetRoles tablolarını oluşturur.
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Diğer DbSet'ler (Identity tabloları otomatik oluşturulur)
        // Kullanıcı tablosu ApplicationUser tarafından sağlandığı için artık bu DbSet'e gerek yoktur.
        public DbSet<Race> Races { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity'ye özgü tabloları oluşturması için bu çağrı ZORUNLUDUR.
            base.OnModelCreating(modelBuilder);

            // =============================================
            // AKIŞKAN API (FLUENT API) KURALLARI
            // =============================================

            // 1. Kayıtlar için Bileşik Tekil Kısıtlama: (user_id, race_id) [unique]
            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.UserId, r.RaceId })
                .IsUnique();

            // 2. Ödeme için Bire-Bir İlişki Kısıtlaması (Unique Foreign Key):
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Registration)
                .WithOne(r => r.PaymentTransaction)
                .HasForeignKey<Payment>(p => p.RegistrationId)
                .IsRequired();

            // 3. Race Entity'sini ApplicationUser (AspNetUsers) ile bağlama
            // CreatedByUserId foreign key'ini doğru bağlama (Yönetici)
            modelBuilder.Entity<Race>()
                .HasOne(r => r.CreatedBy) // Race.CreatedBy
                .WithMany() // ApplicationUser'da bu navigasyon özelliği tanımlı değildir, bu yüzden WithMany() kullanıyoruz
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Payment.TransactionId için Tekil Kısıtlama
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.TransactionId)
                .IsUnique();
        }
    }
}