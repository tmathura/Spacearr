using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Multilarr.Common.Command.MessageCommand.Commands
{
    public class ComputerDrivesCommand : IMessageCommand
    {
        private readonly IDataSize _dataSize;
        private readonly IComputerDrives _computerDrives;

        public ComputerDrivesCommand(IDataSize dataSize, IComputerDrives computerDrives)
        {
            _dataSize = dataSize;
            _computerDrives = computerDrives;
        }

        public CommandObjectSerialized Execute()
        {
            var computerDrives = new List<ComputerDrive>();
            if (_computerDrives.GetComputerDrives().Length > 0)
            {
                foreach (var computerDrive in _computerDrives.GetComputerDrives())
                {
                    computerDrives.Add(new ComputerDrive
                    {
                        Name = computerDrive.Name,
                        RootDirectory = computerDrive.RootDirectory.ToString(),
                        VolumeLabel = computerDrive.VolumeLabel,
                        DriveFormat = computerDrive.DriveFormat,
                        DriveType = computerDrive.DriveType,
                        IsReady = computerDrive.IsReady,
                        TotalFreeSpace = computerDrive.TotalFreeSpace,
                        TotalFreeSpaceString = _dataSize.SizeSuffix(computerDrive.TotalFreeSpace, 2),
                        TotalUsedSpace = computerDrive.TotalSize - computerDrive.TotalFreeSpace,
                        TotalUsedSpaceString = _dataSize.SizeSuffix(computerDrive.TotalSize - computerDrive.TotalFreeSpace, 2),
                        TotalSize = computerDrive.TotalSize,
                        TotalSizeString = _dataSize.SizeSuffix(computerDrive.TotalSize, 2)
                    });
                }
            }

            var messageCommandObject = new CommandObjectSerialized
            {
                SerializeObject = JsonConvert.SerializeObject(computerDrives)
            };
            return messageCommandObject;
        }
    }
}