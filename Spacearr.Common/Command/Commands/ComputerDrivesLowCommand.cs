using Microsoft.Extensions.Configuration;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Common.Command.Commands
{
    public class ComputerDrivesLowCommand : ICommand
    {
        private readonly long _lowComputerDriveValue;

        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;
        private readonly ISendFirebasePushNotification _sendFirebasePushNotification;

        public ComputerDrivesLowCommand(IConfiguration configuration, ILogger logger, IComputerDrives computerDrives, ISendFirebasePushNotification sendFirebasePushNotification)
        {
            _logger = logger;
            _computerDrives = computerDrives;
            _sendFirebasePushNotification = sendFirebasePushNotification;
            _lowComputerDriveValue = Convert.ToInt64(configuration.GetSection("LowComputerDriveGBValue").Value);
        }

        /// <summary>
        /// Returns all the computers hard disk that are low, the low value is determined by the _lowComputerDriveValue.
        /// </summary>
        /// <returns>Returns a NotificationEventArgsModel serialized as Json</returns>
        public string Execute()
        {
            var notificationList = new List<string>();
            var lowDiskSpaceWarningGb = _lowComputerDriveValue;

            if (lowDiskSpaceWarningGb > 0)
            {
                if (_computerDrives.GetComputerDrives().Count > 0)
                {
                    foreach (var drive in _computerDrives.GetComputerDrives())
                    {
                        if (drive.DriveType != DriveType.Fixed) continue;
                        var lowDiskSpaceWarningBytes = lowDiskSpaceWarningGb * 1024 * 1024 * 1024;
                        if (lowDiskSpaceWarningBytes >= drive.TotalFreeSpace)
                        {
                            notificationList.Add($"Disk drive: {drive.VolumeLabel} ({drive.Name}) is low on space.{Environment.NewLine}Total free space: {DataSize.SizeSuffix(drive.TotalFreeSpace, 2)}");
                        }
                    }
                }

                foreach (var notification in notificationList)
                {
                    var deviceTokens = new List<string>();
                    var firebasePushNotificationDevices = _logger.GetFirebasePushNotificationDevicesAsync().Result;
                    foreach (var firebasePushNotificationDevice in firebasePushNotificationDevices)
                    {
                        if (!string.IsNullOrWhiteSpace(firebasePushNotificationDevice.Token))
                        {
                            deviceTokens.Add(firebasePushNotificationDevice.Token);
                        }
                    }

                    if (deviceTokens.Count > 0)
                    {
                        _sendFirebasePushNotification.SendNotificationMultipleDevices(deviceTokens, "Computer Drives Low", notification);
                    }
                }
            }

            return string.Empty;
        }
    }
}