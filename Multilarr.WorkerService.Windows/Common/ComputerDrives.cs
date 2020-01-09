using Multilarr.WorkerService.Windows.Common.Interfaces;
using System.IO;

namespace Multilarr.WorkerService.Windows.Common
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