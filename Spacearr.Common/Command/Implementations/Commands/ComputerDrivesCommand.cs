using Newtonsoft.Json;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class ComputerDrivesCommand : ICommand
    {
        private readonly IComputerDrives _computerDrives;

        public ComputerDrivesCommand(IComputerDrives computerDrives)
        {
            _computerDrives = computerDrives;
        }

        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveModels serialized as Json</returns>
        public async Task<string> Execute()
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

            return await Task.FromResult(JsonConvert.SerializeObject(computerDrives));
        }
    }
}