﻿using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class LowSpaceCommand : ICommand
    {
        private readonly long _lowSpaceGBValue;

        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;
        private readonly ISendFirebasePushNotificationService _sendFirebasePushNotificationService;

        public LowSpaceCommand(IConfiguration configuration, ILogger logger, IComputerDrives computerDrives, ISendFirebasePushNotificationService sendFirebasePushNotificationService)
        {
            _logger = logger;
            _computerDrives = computerDrives;
            _sendFirebasePushNotificationService = sendFirebasePushNotificationService;
            _lowSpaceGBValue = Convert.ToInt64(configuration.GetSection("LowSpaceGBValue").Value);
        }

        /// <summary>
        /// Returns all the computers hard disk that are low, the low value is determined by the _lowSpaceGBValue.
        /// </summary>
        /// <returns>string.Empty</returns>
        public async Task<List<string>> Execute()
        {
            var notificationList = new List<string>();

            if (_lowSpaceGBValue > 0)
            {
                if (_computerDrives.GetComputerDrives().Count > 0)
                {
                    foreach (var drive in _computerDrives.GetComputerDrives())
                    {
                        if (drive.DriveType != DriveType.Fixed) continue;
                        var lowDiskSpaceWarningBytes = _lowSpaceGBValue * 1024 * 1024 * 1024;
                        if (lowDiskSpaceWarningBytes >= drive.TotalFreeSpace)
                        {
                            notificationList.Add($"Disk drive: {drive.VolumeLabel} ({drive.Name}) is low on space.{Environment.NewLine}Total free space: {DataSize.SizeSuffix(drive.TotalFreeSpace, 2)}");
                        }
                    }
                }

                foreach (var notification in notificationList)
                {
                    var deviceTokens = new List<string>();
                    var firebasePushNotificationDevices = await _logger.GetFirebasePushNotificationDevicesAsync();
                    foreach (var firebasePushNotificationDevice in firebasePushNotificationDevices)
                    {
                        if (!string.IsNullOrWhiteSpace(firebasePushNotificationDevice.Token))
                        {
                            deviceTokens.Add(firebasePushNotificationDevice.Token);
                        }
                    }

                    if (deviceTokens.Count > 0)
                    {
                        await _sendFirebasePushNotificationService.SendNotificationMultipleDevices(deviceTokens, "Computer Drives Low", notification);
                    }
                }
            }

            return new List<string>();
        }
    }
}