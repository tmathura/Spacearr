﻿using System;

namespace Multilarr.Notifications
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(string title, string message);

        void ReceiveNotification(int id, string title, string message);
    }
}