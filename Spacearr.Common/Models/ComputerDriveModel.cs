using Microcharts;
using Spacearr.Common.Util;
using System.IO;

namespace Spacearr.Common.Models
{
    public class ComputerDriveModel
    {
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public long TotalFreeSpace { get; set; }
        public string TotalFreeSpaceString => DataSize.SizeSuffix(TotalFreeSpace, 2);
        public long TotalUsedSpace => TotalSize - TotalFreeSpace;
        public string TotalUsedSpaceString => DataSize.SizeSuffix(TotalSize - TotalFreeSpace, 2);
        public long TotalSize { get; set; }
        public string TotalSizeString => DataSize.SizeSuffix(TotalSize, 2);
        public DonutChart Chart { get; set; }
    }
}