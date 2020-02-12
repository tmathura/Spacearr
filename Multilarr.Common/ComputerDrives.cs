using System.IO;
using Multilarr.Common.Interfaces;

namespace Multilarr.Common
{
    public class ComputerDrives : IComputerDrives
    {
        private static IComputerDriveInfo _computerDrive;

        public ComputerDrives(IComputerDriveInfo computerDrive)
        {
            _computerDrive = computerDrive;
        }

        public DriveInfo[] GetComputerDrives()
        {
            return _computerDrive.GetComputerDrives();
        }
    }
}