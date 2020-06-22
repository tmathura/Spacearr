using System.Collections.Generic;

namespace Spacearr.Common.Interfaces
{
    public interface IComputerDriveInfo
    {
        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveInfos</returns>
        List<ComputerDriveInfo> GetComputerDrives();
    }
}