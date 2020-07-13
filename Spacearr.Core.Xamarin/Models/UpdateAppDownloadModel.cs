using Spacearr.Core.Xamarin.Helpers.Interfaces;

namespace Spacearr.Core.Xamarin.Models
{
    public class UpdateAppDownloadModel
    {
        public string Url { get; set; }
        public string VersionNumber { get; set; }

        public IMenuPageHelper MenuPage { get; set; }
    }
}
