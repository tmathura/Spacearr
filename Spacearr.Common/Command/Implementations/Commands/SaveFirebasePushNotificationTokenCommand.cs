using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;

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
        /// <returns></returns>
        public string Execute()
        {
            var firebasePushNotificationDevice = _logger.GetFirebasePushNotificationDeviceAsync(_firebasePushNotificationDevice.DeviceId).Result;
            if (firebasePushNotificationDevice == null)
            {
                _logger.SaveFirebasePushNotificationDeviceAsync(_firebasePushNotificationDevice.DeviceId, _firebasePushNotificationDevice.Token);
            }
            else
            {
                firebasePushNotificationDevice.Token = _firebasePushNotificationDevice.Token;
                _logger.UpdateFirebasePushNotificationDeviceAsync(firebasePushNotificationDevice);
            }

            return string.Empty;
        }
    }
}