using System.Collections.Generic;

namespace Spacearr.Common.Interfaces
{
    public interface IComputerDrives
    {
        List<ComputerDriveInfo> GetComputerDrives();
    }
}