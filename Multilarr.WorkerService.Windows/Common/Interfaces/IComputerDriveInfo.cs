using System.IO;

namespace Multilarr.WorkerService.Windows.Common.Interfaces
{
    public interface IComputerDriveInfo
    {
        DriveInfo[] GetDrives();
    }
}