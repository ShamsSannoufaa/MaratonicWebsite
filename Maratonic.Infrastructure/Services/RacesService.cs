using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maratonic.Infrastructure.Services
{
    public class RacesService : IRacesService
    {
        private readonly AppDbContext _context;

        public RacesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Race>> GetAllRacesAsync()
        {
            return await _context.Races
                .OrderBy(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<Race?> GetRaceByIdAsync(int raceId)
        {
            return await _context.Races
                .FirstOrDefaultAsync(r => r.RaceId == raceId);
        }

        public async Task<Race> CreateRaceAsync(Race race)
        {
            _context.Races.Add(race);
            await _context.SaveChangesAsync();
            return race;
        }

        public async Task<bool> UpdateRaceAsync(Race race)
        {
            var existing = await _context.Races.FirstOrDefaultAsync(r => r.RaceId == race.RaceId);

            if (existing == null)
                return false;

            existing.Title = race.Title;
            existing.Location = race.Location;
            existing.StartDate = race.StartDate;
            existing.Description = race.Description;
            existing.Price = race.Price;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
