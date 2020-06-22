using Spacearr.Common.Interfaces;
using System.Collections.Generic;

namespace Spacearr.Common
{
    public class ComputerDrives : IComputerDrives
    {
        private static IComputerDriveInfo _computerDrive;

        public ComputerDrives(IComputerDriveInfo computerDrive)
        {
            _computerDrive = computerDrive;
        }

        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a list of ComputerDriveInfos</returns>
        public List<ComputerDriveInfo> GetComputerDrives()
        {
            return _computerDrive.GetComputerDrives();
        }
    }
}