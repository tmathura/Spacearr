using Multilarr.Common.Interfaces;
using System.Collections.Generic;

namespace Multilarr.Common
{
    public class ComputerDrives : IComputerDrives
    {
        private static IComputerDriveInfo _computerDrive;

        public ComputerDrives(IComputerDriveInfo computerDrive)
        {
            _computerDrive = computerDrive;
        }

        public List<ComputerDriveInfo> GetComputerDrives()
        {
            return _computerDrive.GetComputerDrives();
        }
    }
}