using Spacearr.Common.Services.Interfaces;
using Spacearr.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Spacearr.UWP.Services
{
    public class FileService : IFileService
    {
        public string GetStorageFolderPath()
        {
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }
    }
}