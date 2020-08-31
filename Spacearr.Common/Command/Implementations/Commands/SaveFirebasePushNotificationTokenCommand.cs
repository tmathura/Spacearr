using System.Collections.Generic;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class SaveFirebasePushNotificationTokenCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly FirebasePushNotificationDevice _firebasePushNotificationDevice;

        public SaveFirebasePushNotificationTokenCommand(ILogger logger, FirebasePushNotificationDevice firebasePushNotificationDevice)
        {
            _logger = logger;
            _firebasePushNotificationDevice = firebasePushNotificationDevice;
        }

        /// <summary>
        /// Save the firebase push notification token.
        /// </summary>
        /// <returns>string.Empty</returns>
        public async Task<List<string>> Execute()
        {
            var firebasePushNotificationDevice = await _logger.GetFirebasePushNotificationDeviceAsync(_firebasePushNotificationDevice.DeviceId);
            if (firebasePushNotificationDevice == null)
            {
                await _logger.SaveFirebasePushNotificationDeviceAsync(_firebasePushNotificationDevice.DeviceId, _firebasePushNotificationDevice.Token);
            }
            else
            {
                firebasePushNotificationDevice.Token = _firebasePushNotificationDevice.Token;
                await _logger.UpdateFirebasePushNotificationDeviceAsync(firebasePushNotificationDevice);
            }

            return new List<string>();
        }
    }
}