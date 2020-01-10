using System.Collections.Generic;

namespace Multilarr.Services
{
    public interface INotificationService
    {
        IEnumerable<NotificationEventArgs> GetNotifications();
    }
}