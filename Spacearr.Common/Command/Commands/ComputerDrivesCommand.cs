using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Spacearr.Common.Command.Commands
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
            var computerDrives = new List<ComputerDriveModel>();
            if (_computerDrives.GetComputerDrives().Count > 0)
            {
                foreach (var computerDrive in _computerDrives.GetComputerDrives())
                {
                    computerDrives.Add(new ComputerDriveModel
                    {
                        Name = computerDrive.Name,
                        RootDirectory = computerDrive.RootDirectory,
                        VolumeLabel = computerDrive.VolumeLabel,
                        DriveFormat = computerDrive.DriveFormat,
                        DriveType = computerDrive.DriveType,
                        IsReady = computerDrive.IsReady,
                        TotalFreeSpace = computerDrive.TotalFreeSpace,
                        TotalSize = computerDrive.TotalSize
                    });
                }
            }

            return JsonConvert.SerializeObject(computerDrives);
        }
    }
}