using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maratonic.Infrastructure.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly AppDbContext _context;

        public RegistrationsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Registration> RegisterAsync(int userId, int raceId)
        {
            // Önce kullanıcı zaten kaydolmuş mu kontrol ediyoruz
            var existing = await _context.Registrations
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RaceId == raceId);

            if (existing != null)
            {
                return existing;  // Zaten kayıtlıysa mevcut kaydı döndür
            }

            var registration = new Registration
            {
                UserId = userId,
                RaceId = raceId,
                RegistrationDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Pending
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return registration;
        }

        public async Task<List<Registration>> GetUserRegistrationsAsync(int userId)
        {
            return await _context.Registrations
                .Include(r => r.Race)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }
    }
}
