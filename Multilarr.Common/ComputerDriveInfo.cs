using System.IO;
using Multilarr.Common.Interfaces;

namespace Multilarr.Common
{
    public class ComputerDriveInfo : IComputerDriveInfo
    {
        public DriveInfo[] GetComputerDrives()
        {
            return DriveInfo.GetDrives();
        }
    }
}