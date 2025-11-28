using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Maratonic.Core.Interfaces;
using Maratonic.Core.Entities;
using Maratonic.API.DTOs;
using Maratonic.Core.Enums;

namespace Maratonic.API.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class RacesController : ControllerBase
    {
        private readonly IRacesService _racesService;

        public RacesController(IRacesService racesService)
        {
            _racesService = racesService;
        }

        // ============================================================
        // GET ALL RACES
        // ============================================================
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var races = await _racesService.GetAllRacesAsync();

            var response = races.Select(r => new RaceResponseDto
            {
                RaceId = r.RaceId,
                Name = r.Name,
                Date = r.Date,
                RegistrationFee = r.RegistrationFee,
                Location = r.Location,
                Description = r.Description,
                Capacity = r.Capacity,
                Status = r.Status
            });

            return Ok(response);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var race = await _racesService.GetRaceByIdAsync(id);
            if (race == null)
                return NotFound("Race not found.");

            return Ok(new RaceResponseDto
            {
                RaceId = race.RaceId,
                Name = race.Name,
                Date = race.Date,
                RegistrationFee = race.RegistrationFee,
                Location = race.Location,
                Description = race.Description,
                Capacity = race.Capacity,
                Status = race.Status
            });
        }

        // ============================================================
        // CREATE RACE
        // ============================================================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RaceCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not found.");

            var race = new Race
            {
                Name = dto.Name,
                Date = dto.Date,
                RegistrationFee = dto.RegistrationFee,
                Location = dto.Location,
                Description = dto.Description,
                Capacity = dto.Capacity,
                Status = RaceStatus.Upcoming,
                CreatedByUserId = userId
            };

            var created = await _racesService.CreateRaceAsync(race);

            return Ok(new
            {
                message = "Race created successfully.",
                created.RaceId
            });
        }

        // ============================================================
        // UPDATE RACE
        // ============================================================
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] RaceUpdateDto dto)
        {
            var race = await _racesService.GetRaceByIdAsync(dto.RaceId);
            if (race == null)
                return NotFound("Race not found.");

            race.Name = dto.Name;
            race.Date = dto.Date;
            race.RegistrationFee = dto.RegistrationFee;
            race.Location = dto.Location;
            race.Description = dto.Description;
            race.Capacity = dto.Capacity;

            var ok = await _racesService.UpdateRaceAsync(race);

            return Ok("Race updated successfully.");
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _racesService.DeleteRaceAsync(id);

            if (!ok)
                return NotFound("Race not found.");

            return Ok("Race deleted successfully.");
        }
    }
}
