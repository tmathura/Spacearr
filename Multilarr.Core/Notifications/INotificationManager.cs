using System;

namespace Multilarr.Core.Notifications
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(string title, string message);

        void ReceiveNotification(int id, string title, string message);
    }
}
