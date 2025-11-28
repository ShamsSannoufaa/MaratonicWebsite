using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using Maratonic.API.DTOs;
using System.Security.Claims;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        // ============================================================
        // GET MY NOTIFICATIONS
        // ============================================================
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _notificationsService.GetUserNotificationsAsync(userId);

            return Ok(list);
        }

        // ============================================================
        // ADMIN: GET ALL NOTIFICATIONS
        // ============================================================
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _notificationsService.GetAllNotificationsAsync();
            return Ok(list);
        }

        // ============================================================
        // ADMIN: CREATE NOTIFICATION FOR ANY USER
        // ============================================================
        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            var notif = await _notificationsService.CreateNotificationAsync(
                dto.UserId,
                dto.Type,
                dto.Subject,
                dto.Body
            );

            return Ok(new
            {
                message = "Notification created.",
                notificationId = notif.NotificationId
            });
        }

        // ============================================================
        // MARK AS SENT
        // ============================================================
        [HttpPost("mark-sent/{id}")]
        public async Task<IActionResult> MarkAsSent(int id)
        {
            var ok = await _notificationsService.MarkAsSentAsync(id);

            if (!ok)
                return BadRequest("Notification not found.");

            return Ok("Notification marked as sent.");
        }
    }

    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
