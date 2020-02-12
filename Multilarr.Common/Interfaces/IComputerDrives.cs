using System.IO;

namespace Multilarr.Common.Interfaces
{
    public interface IComputerDrives
    {
        DriveInfo[] GetComputerDrives();
    }
}