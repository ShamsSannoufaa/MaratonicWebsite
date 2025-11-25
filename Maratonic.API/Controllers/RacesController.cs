using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maratonic.Core.Interfaces;
using Maratonic.Core.Entities;
using Maratonic.API.DTOs;

namespace Maratonic.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RacesController : ControllerBase
    {
        private readonly IRacesService _racesService;

        public RacesController(IRacesService racesService)
        {
            _racesService = racesService;
        }

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
            }).ToList();

            return Ok(response);
        }

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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RaceCreateDto dto)
        {
            var race = new Race
            {
                Name = dto.Name,
                Date = dto.Date,
                RegistrationFee = dto.RegistrationFee,
                Location = dto.Location,
                Description = dto.Description,
                Capacity = dto.Capacity,
                Status = Core.Enums.RaceStatus.Upcoming
            };

            var created = await _racesService.CreateRaceAsync(race);

            return Ok(new
            {
                message = "Race created successfully.",
                created.RaceId
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] RaceUpdateDto dto)
        {
            var race = new Race
            {
                RaceId = dto.RaceId,
                Name = dto.Name,
                Date = dto.Date,
                RegistrationFee = dto.RegistrationFee,
                Location = dto.Location,
                Description = dto.Description,
                Capacity = dto.Capacity
            };

            var ok = await _racesService.UpdateRaceAsync(race);

            if (!ok)
                return NotFound("Race not found.");

            return Ok("Race updated successfully.");
        }

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
