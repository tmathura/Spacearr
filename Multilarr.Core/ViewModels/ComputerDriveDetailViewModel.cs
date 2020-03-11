using Multilarr.Common.Models;

namespace Multilarr.Core.ViewModels
{
    public class ComputerDriveDetailViewModel : BaseViewModel
    {
        public ComputerDriveModel ComputerDriveModel { get; set; }
        public ComputerDriveDetailViewModel(ComputerDriveModel computerDriveModel)
        {
            Title = $"{computerDriveModel?.Name} {computerDriveModel?.VolumeLabel}";
            ComputerDriveModel = computerDriveModel;
        }
    }
}
