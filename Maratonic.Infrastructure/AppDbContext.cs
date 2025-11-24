using Microsoft.EntityFrameworkCore;
using Maratonic.Core.Entities; // Entity sınıflarınızı import ediyoruz

namespace Maratonic.Infrastructure
{
    // AppDbContext, DbContext'ten miras alır ve EF Core'un ana sınıfıdır.
    public class AppDbContext : DbContext
    {
        // Constructor, AppDbContext'i projenin başlangıcında yapılandırmak için kullanılır.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DBML'deki her tablo için bir DbSet (Veritabanı kümesi) tanımlanması
        public DbSet<User> Users { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        // Fluent API ile özel kısıtlamaları ve ilişkileri yapılandırma
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Kayıtlar için Bileşik Tekil Kısıtlama: 
            // DBML'deki Index kısıtlamasını uyguluyoruz: (user_id, race_id) [unique]
            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.UserId, r.RaceId })
                .IsUnique();

            // 2. Ödeme için Tekillik Kısıtlaması:
            // Bir Registration'ın sadece bir Payment'ı olabilir (Bire-Bir ilişki).
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Registration)           // Payment, bir Registration'a sahip
                .WithOne(r => r.PaymentTransaction)    // Registration, bir Payment'a sahip (Registration.cs'de tanımladık)
                .HasForeignKey<Payment>(p => p.RegistrationId) // Foreign Key, Payment tablosunda
                .IsRequired();                         // registration_id [unique, not null] kısıtlaması

            // 3. User Entity'sindeki ilişkiyi düzeltme (Self-Reference için)
            // CreatedByUserId foreign key'ini doğru bağlama
            modelBuilder.Entity<Race>()
                .HasOne(r => r.CreatedBy)
                .WithMany(u => u.CreatedRaces)
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Yönetici silinirse yarışlar silinmesin.

            // 4. Diğer Entity'lerdeki tekil kısıtlamaları uyguluyoruz (Data Annotations ile yapılmayanlar)
            // User.Email'i zaten [Index] ile User.cs'de halletmiştik.

            base.OnModelCreating(modelBuilder);
        }
    }
}