using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Multilarr.WorkerService.Windows.Command.MessageCommand.Commands
{
    public class ComputerDrivesLowCommand : IMessageCommand
    {
        private readonly IDataSize _dataSize;
        private readonly long _lowDiskSpaceWarning;
        private readonly IComputerDrives _windowsDrives;

        public ComputerDrivesLowCommand(IDataSize dataSize, IComputerDrives windowsDrives)
        {
            _dataSize = dataSize;
            _windowsDrives = windowsDrives;
            _lowDiskSpaceWarning = 10;
        }

        public CommandObjectSerialized Execute()
        {
            NotificationEventArgs notificationEventArgs = null;
            var lowDiskSpaceWarningGb = _lowDiskSpaceWarning;

            if (lowDiskSpaceWarningGb > 0)
            {
                var notificationList = new List<string>();
                if (_windowsDrives.GetComputerDrives().Length > 0)
                {
                    foreach (var drive in _windowsDrives.GetComputerDrives())
                    {
                        if (drive.DriveType != DriveType.Fixed) continue;
                        var lowDiskSpaceWarningBytes = lowDiskSpaceWarningGb * 1024 * 1024 * 1024;
                        if (lowDiskSpaceWarningBytes >= drive.TotalFreeSpace)
                        {
                            notificationList.Add($"Disk drive: {drive.VolumeLabel} ({drive.Name}) is low on space.{Environment.NewLine}Total free space: {_dataSize.SizeSuffix(drive.TotalFreeSpace, 2)}");
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

            var messageCommandObject = new CommandObjectSerialized
            {
                SerializeObject = JsonConvert.SerializeObject(notificationEventArgs)
            };
            return messageCommandObject;
        }
    }
}