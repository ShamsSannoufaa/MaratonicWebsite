using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Maratonic.Infrastructure.Identity;
using Maratonic.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        // =========================================================
        // REGISTER
        // =========================================================
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            // Varsayılan kullanıcı rolü → ATHLETE
            await _userManager.AddToRoleAsync(user, "ATHLETE");

            return Ok(new
            {
                message = "User registered successfully.",
                userId = user.Id
            });
        }

        // =========================================================
        // LOGIN
        // =========================================================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("User not found");

            var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!passwordValid.Succeeded)
                return Unauthorized("Invalid password");

            var token = await GenerateJwtToken(user);

            return Ok(new
            {
                token,
                userId = user.Id,
                email = user.Email
            });
        }

        // =========================================================
        // JWT TOKEN GENERATION
        // =========================================================
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSection = _config.GetSection("Jwt");

            var secret = jwtSection["Secret"];
            if (string.IsNullOrEmpty(secret))
                throw new Exception("Jwt:Secret is missing in appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // Kullanıcı rollerini JWT içine ekle
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                expires: DateTime.UtcNow.AddHours(12),
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
