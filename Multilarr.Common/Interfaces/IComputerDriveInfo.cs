using System.IO;

namespace Multilarr.Common.Interfaces
{
    public interface IComputerDriveInfo
    {
        DriveInfo[] GetComputerDrives();
    }
}