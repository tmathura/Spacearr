using Multilarr.Common.Models;

namespace Multilarr.ViewModels
{
    public class ComputerDriveDetailViewModel : BaseViewModel
    {
        public ComputerDrive ComputerDrive { get; set; }
        public ComputerDriveDetailViewModel(ComputerDrive computerDrive = null)
        {
            Title = $"{computerDrive?.Name} {computerDrive?.VolumeLabel}";
            ComputerDrive = computerDrive;
        }
    }
}
