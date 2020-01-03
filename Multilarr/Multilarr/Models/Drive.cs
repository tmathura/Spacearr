using System.IO;

namespace Multilarr.Models
{
    public class Drive
    {
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public long TotalFreeSpace { get; set; }
        public long TotalSize { get; set; }

        public float ProgressBarValue => (TotalSize > 0 && TotalFreeSpace > 0 ? (float) TotalFreeSpace / TotalSize : 1);
    }
}