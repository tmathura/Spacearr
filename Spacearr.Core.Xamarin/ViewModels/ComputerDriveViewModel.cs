using Spacearr.Common.Models;
using System.Collections.Generic;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class ComputerDriveViewModel : BaseViewModel
    {
        public List<ComputerDriveModel> ComputerDriveModel { get; set; }
        public ComputerDriveViewModel(ComputerModel computerModel)
        {
            Title = computerModel.Name;
            ComputerDriveModel = computerModel.ComputerDrives;
        }
    }
}
