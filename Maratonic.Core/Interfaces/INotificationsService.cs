using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces
{
    public interface INotificationsService
    {
        Task<Notification> CreateNotificationAsync(string userId, string type, string subject, string body);
    }
}
