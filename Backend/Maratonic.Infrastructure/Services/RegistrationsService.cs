using Maratonic.Core.Entities;
using Maratonic.Core.Enums;
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

        // ============================================================
        // REGISTER USER TO RACE
        // ============================================================
        public async Task<Registration> RegisterAsync(string userId, int raceId)
        {
            var race = await _context.Races.FirstOrDefaultAsync(r => r.RaceId == raceId);
            if (race == null)
                throw new Exception("Race not found.");

            // Duplicate kayıt kontrolü
            bool alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.UserId == userId && r.RaceId == raceId);

            if (alreadyRegistered)
                throw new Exception("User is already registered for this race.");

            // Capacity kontrolü
            int currentCount = await _context.Registrations
                .CountAsync(r => r.RaceId == raceId);

            if (race.Capacity > 0 && currentCount >= race.Capacity)
                throw new Exception("Race capacity is full.");

            var registration = new Registration
            {
                UserId = userId,
                RaceId = raceId,
                Fee = race.RegistrationFee,
                PaymentStatus = PaymentStatus.Pending,
                RegistrationDate = DateTime.UtcNow
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return registration;
        }

        public async Task<List<Registration>> GetUserRegistrationsAsync(string userId)
        {
            return await _context.Registrations
                .Include(r => r.Race)
                .Include(r => r.PaymentTransaction)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Race)
                .Include(r => r.PaymentTransaction)
                .ToListAsync();
        }
    }
}
