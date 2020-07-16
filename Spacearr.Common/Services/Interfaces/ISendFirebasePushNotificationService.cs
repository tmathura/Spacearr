using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface ISendFirebasePushNotificationService
    {
        /// <summary>
        /// Send a firebase push notification one device.
        /// </summary>
        /// <returns></returns>
        Task SendNotification(string token, string notificationTitle, string notificationMessage);

        /// <summary>
        /// Send a firebase push notification to multiple devices.
        /// </summary>
        /// <returns></returns>
        Task SendNotificationMultipleDevices(List<string> tokens, string notificationTitle, string notificationMessage);
    }
}