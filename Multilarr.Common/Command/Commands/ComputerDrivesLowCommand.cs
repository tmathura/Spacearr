using Microsoft.Extensions.Configuration;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Models;
using Multilarr.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Multilarr.Common.Command.Commands
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

        public string Execute()
        {
            NotificationEventArgs notificationEventArgs = null;
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
                    notificationEventArgs = new NotificationEventArgs
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