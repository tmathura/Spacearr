using Newtonsoft.Json;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class GetComputerDrivesCommand : ICommand
    {
        private readonly IComputerDrives _computerDrives;

        public GetComputerDrivesCommand(IComputerDrives computerDrives)
        {
            _computerDrives = computerDrives;
        }

        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveModels serialized as Json</returns>
        public async Task<List<string>> Execute()
        {
            var commandData = new List<string>();

            if (_computerDrives.GetComputerDrives().Count > 0)
            {
                commandData.AddRange(_computerDrives.GetComputerDrives().Select(computerDrive => JsonConvert.SerializeObject(new ComputerDriveModel
                {
                    Name = computerDrive.Name,
                    RootDirectory = computerDrive.RootDirectory,
                    VolumeLabel = computerDrive.VolumeLabel,
                    DriveFormat = computerDrive.DriveFormat,
                    DriveType = computerDrive.DriveType,
                    IsReady = computerDrive.IsReady,
                    TotalFreeSpace = computerDrive.TotalFreeSpace,
                    TotalSize = computerDrive.TotalSize
                })));
            }
            
            return await Task.FromResult(commandData);
        }
    }
}