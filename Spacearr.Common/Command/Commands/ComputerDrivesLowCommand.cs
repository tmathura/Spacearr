using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Models;
using Spacearr.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Common.Command.Commands
{
    public class ComputerDrivesLowCommand : ICommand
    {
        private readonly long _lowComputerDriveValue;

        private readonly IComputerDrives _computerDrives;

        public ComputerDrivesLowCommand(IConfiguration configuration, IComputerDrives computerDrives)
        {
            _computerDrives = computerDrives;
            _lowComputerDriveValue = Convert.ToInt64(configuration.GetSection("LowComputerDriveGBValue").Value);
        }

        /// <summary>
        /// Returns all the computers hard disk that are low, the low value is determined by the _lowComputerDriveValue.
        /// </summary>
        /// <returns>Returns a NotificationEventArgsModel serialized as Json</returns>
        public string Execute()
        {
            NotificationEventArgsModel notificationEventArgs = null;
            var lowDiskSpaceWarningGb = _lowComputerDriveValue;

            if (lowDiskSpaceWarningGb > 0)
            {
                var notificationList = new List<string>();
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

                if (notificationList.Count > 0)
                {
                    notificationEventArgs = new NotificationEventArgsModel
                    {
                        Title = "Computer Drives Low",
                        Message = string.Join(Environment.NewLine, notificationList.ToArray())
                    };
                }
            }

            return JsonConvert.SerializeObject(notificationEventArgs);
        }
    }
}