using System.IO;

namespace Multilarr.WorkerService.Windows.Common.Interfaces
{
    public interface IComputerDrives
    {
        DriveInfo[] GetComputerDrives();
    }
}