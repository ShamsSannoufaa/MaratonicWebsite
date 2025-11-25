using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Maratonic.Infrastructure.Identity;
using Maratonic.Core.Entities;

namespace Maratonic.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Race> Races { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Registration: Unique (UserId, RaceId)
            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.UserId, r.RaceId })
                .IsUnique();

            // 2. Payment <-> Registration (1-to-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Registration)
                .WithOne(r => r.PaymentTransaction)
                .HasForeignKey<Payment>(p => p.RegistrationId)
                .IsRequired();

            // 3. Payment.TransactionId unique
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.TransactionId)
                .IsUnique();
        }
    }
}
