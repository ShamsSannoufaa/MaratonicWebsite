using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;

namespace Maratonic.Infrastructure.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;

        public NotificationsService(AppDbContext context)
        {
            _context = context;
        }

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
    }
}
