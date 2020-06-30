using System.Collections.Generic;
using System.Linq;

namespace Spacearr.Common.Models
{
    public class ComputerModel
    {
        public string Name { get; set; }
        public List<ComputerDriveModel> ComputerDrives { get; set; }
        public float ProgressBarValue => HasError ? 0 : ComputerDrives.Sum(x => x.TotalSize) > 0 && ComputerDrives.Sum(x => x.TotalFreeSpace) > 0 ? (float)(ComputerDrives.Sum(x => x.TotalSize) - ComputerDrives.Sum(x => x.TotalFreeSpace)) / ComputerDrives.Sum(x => x.TotalSize) : 1;
        public bool Online => !HasError;
        public bool HasError => !string.IsNullOrWhiteSpace(Error);
        public string Error { get; set; }
    }
}