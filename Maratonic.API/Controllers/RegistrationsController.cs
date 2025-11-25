using System;
using Maratonic.API.DTOs;
using Maratonic.Core.Entities;
using Maratonic.Core.Enums;
using Maratonic.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("register")]
        [Authorize(Roles = "Athlete,Admin")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var race = await _racesService.GetRaceByIdAsync(dto.RaceId);
            if (race == null)
                return NotFound("Race not found.");

            var registration = await _registrationsService.RegisterAsync(userId, dto.RaceId);

            return Ok(new
            {
                message = "Registration successful.",
                registrationId = registration.RegistrationId,
                paymentStatus = registration.PaymentStatus.ToString()
            });
        }

        [HttpGet("my")]
        [Authorize(Roles = "Athlete,Admin")]
        public async Task<IActionResult> MyRegistrations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var registrations = await _registrationsService.GetUserRegistrationsAsync(userId);

            var response = registrations.Select(r => new
            {
                r.RegistrationId,
                r.RaceId,
                r.Race.Name,
                r.Race.Date,
                r.RegistrationDate,
                PaymentStatus = r.PaymentStatus.ToString()
            });

            return Ok(response);
        }
    }
}
