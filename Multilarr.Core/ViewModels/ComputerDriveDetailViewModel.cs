using Multilarr.Common.Models;

namespace Multilarr.Core.ViewModels
{
    public class ComputerDriveDetailViewModel : BaseViewModel
    {
        public ComputerDriveModel ComputerDrive { get; set; }
        public ComputerDriveDetailViewModel(ComputerDriveModel computerDrive = null)
        {
            Title = $"{computerDrive?.Name} {computerDrive?.VolumeLabel}";
            ComputerDrive = computerDrive;
        }
    }
}
