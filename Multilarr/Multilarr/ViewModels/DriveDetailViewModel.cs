using Multilarr.Models;

namespace Multilarr.ViewModels
{
    public class DriveDetailViewModel : BaseViewModel
    {
        public Drive Drive { get; set; }
        public DriveDetailViewModel(Drive drive = null)
        {
            Title = $"{drive?.Name}: {drive?.VolumeLabel}";
            Drive = drive;
        }
    }
}
