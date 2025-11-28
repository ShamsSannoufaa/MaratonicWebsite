using Microsoft.EntityFrameworkCore;
using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;
using Maratonic.Infrastructure;
namespace Maratonic.Infrastructure.Services
{
    public class RacesService : IRacesService
    {
        private readonly AppDbContext _context;

        public RacesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Race>> GetAllRacesAsync()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race?> GetRaceByIdAsync(int raceId)
        {
            return await _context.Races.FirstOrDefaultAsync(r => r.RaceId == raceId);
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

            _context.Entry(existing).CurrentValues.SetValues(race);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRaceAsync(int raceId)
        {
            var race = await _context.Races.FindAsync(raceId);

            if (race == null)
                return false;

            _context.Races.Remove(race);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
