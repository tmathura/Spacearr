using Spacearr.Common.Models;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class ComputerDriveDetailViewModel : BaseViewModel
    {
        public ComputerDriveModel ComputerDriveModel { get; set; }
        public ComputerDriveDetailViewModel(ComputerDriveModel computerDriveModel)
        {
            Title = $"{computerDriveModel.Name} {computerDriveModel.VolumeLabel}";
            ComputerDriveModel = computerDriveModel;
        }
    }
}
