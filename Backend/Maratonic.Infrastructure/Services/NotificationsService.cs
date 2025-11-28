using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maratonic.Infrastructure.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;

        public NotificationsService(AppDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Notification> CreateNotificationAsync(string userId, string type, string subject, string body)
        {
            var notif = new Notification
            {
                UserId = userId,
                Type = type,
                Subject = subject,
                Body = body,
                SentAt = null,
                IsSent = false
            };

            _context.Notifications.Add(notif);
            await _context.SaveChangesAsync();

            return notif;
        }

        // GET MY NOTIFICATIONS
        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.NotificationId)
                .ToListAsync();
        }

        // GET ALL (ADMIN)
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .OrderByDescending(n => n.NotificationId)
                .ToListAsync();
        }

        // MARK AS SENT
        public async Task<bool> MarkAsSentAsync(int notificationId)
        {
            var notif = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId);
            if (notif == null) return false;

            notif.IsSent = true;
            notif.SentAt = DateTime.UtcNow;

            _context.Notifications.Update(notif);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
