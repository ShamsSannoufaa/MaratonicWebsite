using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces;

public interface IRacesService
{
    Task<List<Race>> GetAllRacesAsync();
    Task<Race?> GetRaceByIdAsync(int raceId);
    Task<Race> CreateRaceAsync(Race race);
    Task<bool> UpdateRaceAsync(Race race);
}
