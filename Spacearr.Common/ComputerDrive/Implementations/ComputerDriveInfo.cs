using Spacearr.Common.ComputerDrive.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Common.ComputerDrive.Implementations
{
    public class ComputerDriveInfo : IComputerDriveInfo
    {
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public long TotalFreeSpace { get; set; }
        public long TotalSize { get; set; }

        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveInfos</returns>
        public List<ComputerDriveInfo> GetComputerDrives()
        {
            var drives = DriveInfo.GetDrives();
            var drivesList = new List<ComputerDriveInfo>();

            foreach (var driveInfo in drives)
            {
                drivesList.Add(new ComputerDriveInfo
                {
                    Name = driveInfo.Name,
                    RootDirectory = driveInfo.RootDirectory.ToString(),
                    VolumeLabel = driveInfo.VolumeLabel,
                    DriveFormat = driveInfo.DriveFormat,
                    DriveType = driveInfo.DriveType,
                    IsReady = driveInfo.IsReady,
                    TotalFreeSpace = driveInfo.TotalFreeSpace,
                    TotalSize = driveInfo.TotalSize
                });
            }

            return drivesList;
        }
    }
}