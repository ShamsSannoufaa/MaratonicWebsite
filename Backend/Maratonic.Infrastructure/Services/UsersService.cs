using Microsoft.AspNetCore.Identity;
using Maratonic.Infrastructure.Identity;
using Maratonic.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maratonic.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        // ============================================
        // UPDATE PROFILE
        // ============================================
        public async Task<bool> UpdateProfileAsync(string id, ApplicationUser dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // ============================================
        // CHANGE PASSWORD
        // ============================================
        public async Task<bool> ChangePasswordAsync(string userId, string oldPw, string newPw)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, oldPw, newPw);
            return result.Succeeded;
        }
    }
}
