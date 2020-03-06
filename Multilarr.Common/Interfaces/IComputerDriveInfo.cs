using System.Collections.Generic;

namespace Multilarr.Common.Interfaces
{
    public interface IComputerDriveInfo
    {
        List<ComputerDriveInfo> GetComputerDrives();
    }
}