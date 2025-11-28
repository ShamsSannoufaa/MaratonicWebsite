using System.Security.Claims;
using Maratonic.API.DTOs;
using Maratonic.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maratonic.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationsService _registrationsService;
        private readonly IRacesService _racesService;

        public RegistrationsController(
            IRegistrationsService registrationsService,
            IRacesService racesService)
        {
            _registrationsService = registrationsService;
            _racesService = racesService;
        }

        // ============================================================
        // REGISTER USER TO A RACE
        // ============================================================
        [HttpPost("register")]
        [Authorize(Roles = "ATHLETE,ADMIN")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated.");

            var race = await _racesService.GetRaceByIdAsync(dto.RaceId);
            if (race == null)
                return NotFound("Race not found.");

            try
            {
                var registration = await _registrationsService.RegisterAsync(userId, dto.RaceId);

                return Ok(new RegistrationResponseDto
                {
                    RegistrationId = registration.Id,
                    RaceId = registration.RaceId,
                    RaceName = race.Name,
                    Fee = race.RegistrationFee,
                    PaymentStatus = registration.PaymentStatus.ToString(),
                    RegistrationDate = registration.RegistrationDate
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already registered"))
                    return Conflict("You are already registered for this race.");

                if (ex.Message.Contains("capacity", StringComparison.OrdinalIgnoreCase))
                    return Conflict("Race capacity is full.");

                return BadRequest(ex.Message);
            }
        }

        // ============================================================
        // USER: GET MY REGISTRATIONS
        // ============================================================
        [HttpGet("my")]
        [Authorize(Roles = "ATHLETE,ADMIN")]
        public async Task<IActionResult> MyRegistrations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated.");

            var registrations = await _registrationsService.GetUserRegistrationsAsync(userId);

            var response = registrations.Select(r => new
            {
                registrationId = r.Id,
                r.RaceId,
                RaceName = r.Race?.Name,
                RaceDate = r.Race?.Date,
                Fee = r.Race?.RegistrationFee ?? 0,
                r.RegistrationDate,
                PaymentStatus = r.PaymentStatus.ToString(),

                payment = r.PaymentTransaction == null ? null : new
                {
                    r.PaymentTransaction.Id,
                    r.PaymentTransaction.Amount,
                    r.PaymentTransaction.Status,
                    r.PaymentTransaction.TransactionId,
                    r.PaymentTransaction.CreatedAt,
                    r.PaymentTransaction.RefundedAt
                }
            });

            return Ok(response);
        }

        // ============================================================
        // ADMIN: GET ALL REGISTRATIONS
        // ============================================================
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationsService.GetAllRegistrationsAsync();

            var response = registrations.Select(r => new
            {
                registrationId = r.Id,
                r.UserId,
                race = new
                {
                    r.RaceId,
                    r.Race.Name,
                    r.Race.Date
                },
                Fee = r.Race?.RegistrationFee ?? 0,
                payment = r.PaymentTransaction == null ? null : new
                {
                    r.PaymentTransaction.Id,
                    r.PaymentTransaction.Amount,
                    r.PaymentTransaction.Status,
                    r.PaymentTransaction.TransactionId
                },
                RegistrationDate = r.RegistrationDate,
                PaymentStatus = r.PaymentStatus.ToString()
            });

            return Ok(response);
        }
    }
}
