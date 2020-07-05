using System.Collections.Generic;

namespace Spacearr.Common.Interfaces
{
    public interface ISendFirebasePushNotification
    {
        /// <summary>
        /// Send a firebase push notification one device.
        /// </summary>
        /// <returns></returns>
        void SendNotification(string token, string notificationTitle, string notificationMessage);

        /// <summary>
        /// Send a firebase push notification to multiple devices.
        /// </summary>
        /// <returns></returns>
        void SendNotificationMultipleDevices(List<string> tokens, string notificationTitle, string notificationMessage);
    }
}