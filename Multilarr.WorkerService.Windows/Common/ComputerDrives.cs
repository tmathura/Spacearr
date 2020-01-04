using System.IO;
using Multilarr.WorkerService.Windows.Common.Interfaces;

namespace Multilarr.WorkerService.Windows.Common
{
    public class ComputerDrives : IComputerDrives
    {
        private static IComputerDriveInfo _drive;

        public ComputerDrives(IComputerDriveInfo drive)
        {
            _drive = drive;
        }

        public DriveInfo[] GetDrives()
        {
            return _drive.GetDrives();
        }
    }
}