using Maratonic.Infrastructure.Identity;

namespace Maratonic.Core.Interfaces
{
    public interface IUsersService
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);

        // Yeni eklenenler:
        Task<bool> UpdateProfileAsync(string id, ApplicationUser dto);
        Task<bool> ChangePasswordAsync(string userId, string oldPw, string newPw);
    }
}
