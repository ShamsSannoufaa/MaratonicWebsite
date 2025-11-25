using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using Maratonic.API.DTOs;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _usersService.GetAllAsync();

            var dto = users.Select(u => new UserSummaryDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = $"{u.FirstName} {u.LastName}"
            })
            .ToList();

            return Ok(dto);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _usersService.GetByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(new UserSummaryDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}"
            });
        }
    }
}
