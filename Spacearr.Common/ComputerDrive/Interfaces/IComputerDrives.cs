using Spacearr.Common.ComputerDrive.Implementations;
using System.Collections.Generic;

namespace Spacearr.Common.ComputerDrive.Interfaces
{
    public interface IComputerDrives
    {
        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveInfos</returns>
        List<ComputerDriveInfo> GetComputerDrives();
    }
}