using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Services.Interfaces
{
    public interface ISaveFirebasePushNotificationTokenService
    {
        /// <summary>
        /// Save the firebase push notification token.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerModel</returns>
        Task SaveFirebasePushNotificationToken(Guid deviceId, string token);
    }
}
