using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces
{
    public interface INotificationsService
    {
        Task<Notification> CreateNotificationAsync(string userId, string type, string subject, string body);
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<bool> MarkAsSentAsync(int notificationId);
    }
}
