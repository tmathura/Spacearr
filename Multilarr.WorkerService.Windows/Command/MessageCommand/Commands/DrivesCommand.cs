using System.Collections.Generic;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using Multilarr.WorkerService.Windows.Models;

namespace Multilarr.WorkerService.Windows.Command.MessageCommand.Commands
{
    public class DrivesCommand : IMessageCommand
    {
        private readonly IDataSize _dataSize;
        private readonly IComputerDrives _computerDrives;

        public DrivesCommand(IDataSize dataSize, IComputerDrives computerDrives)
        {
            _dataSize = dataSize;
            _computerDrives = computerDrives;
        }

        public MessageCommandObject Execute()
        {
            var drives = new List<Drive>();
            if (_computerDrives.GetDrives().Length > 0)
            {
                foreach (var drive in _computerDrives.GetDrives())
                {
                    drives.Add(new Drive
                    {
                        Name = drive.Name,
                        RootDirectory = drive.RootDirectory.ToString(),
                        VolumeLabel = drive.VolumeLabel,
                        DriveFormat = drive.DriveFormat,
                        DriveType = drive.DriveType,
                        IsReady = drive.IsReady,
                        TotalFreeSpace = drive.TotalFreeSpace,
                        TotalFreeSpaceString = _dataSize.SizeSuffix(drive.TotalFreeSpace, 2),
                        TotalSize = drive.TotalSize,
                        TotalSizeString = _dataSize.SizeSuffix(drive.TotalSize, 2)
                    });
                }
            }

            var messageCommandObject = new MessageCommandObject
            {
                MessageObject = drives
            };
            return messageCommandObject;
        }
    }
}