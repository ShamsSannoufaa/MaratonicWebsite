using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces
{
    public interface IRegistrationsService
    {
        Task<Registration> RegisterAsync(string userId, int raceId);
        Task<List<Registration>> GetUserRegistrationsAsync(string userId);
        Task<List<Registration>> GetAllRegistrationsAsync();
    }
}
