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

        public List<ComputerDriveInfo> GetComputerDrives()
        {
            return _computerDrive.GetComputerDrives();
        }
    }
}