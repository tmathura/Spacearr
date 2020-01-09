﻿using Multilarr.WorkerService.Windows.Common.Interfaces;
using System.IO;

namespace Multilarr.WorkerService.Windows.Common
{
    public class ComputerDriveInfo : IComputerDriveInfo
    {
        public DriveInfo[] GetComputerDrives()
        {
            return DriveInfo.GetDrives();
        }
    }
}