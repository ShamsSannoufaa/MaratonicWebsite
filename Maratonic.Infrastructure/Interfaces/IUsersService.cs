using Maratonic.Infrastructure.Identity;

namespace Maratonic.Core.Interfaces
{
    public interface IUsersService
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
    }
}
