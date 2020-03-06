using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Models;
using Multilarr.Common.Util;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Multilarr.Common.Command.Commands
{
    public class ComputerDrivesCommand : ICommand
    {
        private readonly IComputerDrives _computerDrives;

        public ComputerDrivesCommand(IComputerDrives computerDrives)
        {
            _computerDrives = computerDrives;
        }

        public string Execute()
        {
            var computerDrives = new List<ComputerDrive>();
            if (_computerDrives.GetComputerDrives().Count > 0)
            {
                foreach (var computerDrive in _computerDrives.GetComputerDrives())
                {
                    computerDrives.Add(new ComputerDrive
                    {
                        Name = computerDrive.Name,
                        RootDirectory = computerDrive.RootDirectory,
                        VolumeLabel = computerDrive.VolumeLabel,
                        DriveFormat = computerDrive.DriveFormat,
                        DriveType = computerDrive.DriveType,
                        IsReady = computerDrive.IsReady,
                        TotalFreeSpace = computerDrive.TotalFreeSpace,
                        TotalFreeSpaceString = DataSize.SizeSuffix(computerDrive.TotalFreeSpace, 2),
                        TotalUsedSpace = computerDrive.TotalSize - computerDrive.TotalFreeSpace,
                        TotalUsedSpaceString = DataSize.SizeSuffix(computerDrive.TotalSize - computerDrive.TotalFreeSpace, 2),
                        TotalSize = computerDrive.TotalSize,
                        TotalSizeString = DataSize.SizeSuffix(computerDrive.TotalSize, 2)
                    });
                }
            }

            return JsonConvert.SerializeObject(computerDrives);
        }
    }
}