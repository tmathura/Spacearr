using System.Collections.Generic;

namespace Multilarr.Common.Interfaces
{
    public interface IComputerDrives
    {
        List<ComputerDriveInfo> GetComputerDrives();
    }
}