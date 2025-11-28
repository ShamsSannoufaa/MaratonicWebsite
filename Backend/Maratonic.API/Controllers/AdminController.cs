using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using Maratonic.API.DTOs;
using Maratonic.Core.Enums;
using System.Linq;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IRacesService _racesService;
        private readonly IRegistrationsService _registrationsService;
        private readonly IPaymentsService _paymentsService;
        private readonly INotificationsService _notificationsService;

        public AdminController(
            IUsersService usersService,
            IRacesService racesService,
            IRegistrationsService registrationsService,
            IPaymentsService paymentsService,
            INotificationsService notificationsService)
        {
            _usersService = usersService;
            _racesService = racesService;
            _registrationsService = registrationsService;
            _paymentsService = paymentsService;
            _notificationsService = notificationsService;
        }

        // ====================================================================
        // USERS
        // ====================================================================
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
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

        // ====================================================================
        // RACES
        // ====================================================================
        [HttpGet("races")]
        public async Task<IActionResult> GetAllRaces()
        {
            var races = await _racesService.GetAllRacesAsync();
            return Ok(races);
        }

        // ====================================================================
        // REGISTRATIONS
        // ====================================================================
        [HttpGet("registrations")]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var regs = await _registrationsService.GetAllRegistrationsAsync();
            return Ok(regs.Select(r => new
            {
                r.Id,
                r.UserId,
                r.RaceId,
                RaceName = r.Race?.Name,
                r.RegistrationDate,
                PaymentStatus = r.PaymentStatus.ToString()
            }));
        }

        // ====================================================================
        // PAYMENTS
        // ====================================================================
        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var pays = await _paymentsService.GetAllPaymentsAsync();
            return Ok(pays.Select(p => new
            {
                p.Id,
                p.RegistrationId,
                p.Amount,
                Status = p.Status.ToString(),
                p.TransactionId,
                p.CreatedAt,
                p.RefundedAt
            }));
        }

        // ADMIN MANUAL PAYMENT APPROVAL
        [HttpPost("payments/approve/{registrationId:int}")]
        public async Task<IActionResult> ApprovePayment(int registrationId)
        {
            var payment = await _paymentsService.ApprovePaymentForRegistrationAsync(registrationId);

            return Ok(new
            {
                message = "Payment approved successfully.",
                paymentId = payment.Id,
                registrationId = payment.RegistrationId,
                status = payment.Status.ToString(),
                amount = payment.Amount,
                transactionId = payment.TransactionId
            });
        }

        // ====================================================================
        // NOTIFICATIONS
        // ====================================================================
        [HttpGet("notifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifs = await _notificationsService.GetAllNotificationsAsync();
            return Ok(notifs);
        }

        [HttpPost("notifications/create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request body");

            var notif = await _notificationsService.CreateNotificationAsync(
                dto.UserId,
                dto.Type,
                dto.Subject,
                dto.Body
            );

            return Ok(new
            {
                message = "Notification created",
                notif.NotificationId
            });
        }

        // ====================================================================
        // DASHBOARD / ANALYTICS
        // ====================================================================
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var users = await _usersService.GetAllAsync();
            var races = await _racesService.GetAllRacesAsync();
            var registrations = await _registrationsService.GetAllRegistrationsAsync();
            var payments = await _paymentsService.GetAllPaymentsAsync();

            var regList = registrations.ToList();
            var payList = payments.ToList();
            var raceList = races.ToList();

            return Ok(new
            {
                totalUsers = users.Count,
                totalRaces = raceList.Count,
                totalRegistrations = regList.Count,
                totalPayments = payList.Count,

                paidRegistrations = regList.Count(r => r.PaymentStatus == PaymentStatus.Completed),
                pendingRegistrations = regList.Count(r => r.PaymentStatus == PaymentStatus.Pending),

                completedPayments = payList.Count(p => p.Status == PaymentStatus.Completed),
                refundedPayments = payList.Count(p => p.Status == PaymentStatus.Refunded),
                totalAmountCollected = payList
                    .Where(p => p.Status == PaymentStatus.Completed)
                    .Sum(p => p.Amount),

                lastRegistrations = regList
                    .OrderByDescending(r => r.RegistrationDate)
                    .Take(5)
                    .Select(r => new
                    {
                        r.Id,
                        r.UserId,
                        r.RaceId,
                        RaceName = r.Race?.Name,
                        r.RegistrationDate,
                        PaymentStatus = r.PaymentStatus.ToString()
                    }),

                lastRaces = raceList
                    .OrderByDescending(r => r.Date)
                    .Take(5)
                    .Select(r => new
                    {
                        r.RaceId,
                        r.Name,
                        r.Date,
                        r.Location,
                        r.Capacity
                    }),

                topRacesByRegistrations = regList
                    .Where(r => r.Race != null)
                    .GroupBy(r => new { r.RaceId, r.Race!.Name, r.Race.Capacity })
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new
                    {
                        g.Key.RaceId,
                        g.Key.Name,
                        g.Key.Capacity,
                        registrationCount = g.Count(),
                        remainingCapacity = g.Key.Capacity - g.Count()
                    })
            });
        }
    }
}
