using System.IO;
using Multilarr.WorkerService.Windows.Common.Interfaces;

namespace Multilarr.WorkerService.Windows.Common
{
    public class ComputerDriveInfo : IComputerDriveInfo
    {
        public DriveInfo[] GetDrives()
        {
            return DriveInfo.GetDrives();
        }
    }
}