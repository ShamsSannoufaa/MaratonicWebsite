using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using System.Security.Claims;
using Maratonic.Infrastructure.Identity;
using Maratonic.API.DTOs;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // ===============================================
        // ME → CURRENT USER PROFILE
        // ===============================================
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _usersService.GetByIdAsync(userId);

            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.CreatedAt
            });
        }

        // ===============================================
        // UPDATE PROFILE
        // ===============================================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tempUser = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var success = await _usersService.UpdateProfileAsync(userId, tempUser);

            if (!success)
                return BadRequest("Profile update failed");

            return Ok("Profile updated successfully.");
        }

        // ===============================================
        // CHANGE PASSWORD
        // ===============================================
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _usersService.ChangePasswordAsync(
                userId,
                dto.OldPassword,
                dto.NewPassword
            );

            if (!success)
                return BadRequest("Password change failed");

            return Ok("Password changed successfully.");
        }

        // ===============================================
        // ADMIN → ALL USERS
        // ===============================================
        [HttpGet("list")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> List()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users.Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.CreatedAt
            }));
        }
    }
}
