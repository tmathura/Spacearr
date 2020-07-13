using Spacearr.Common.Services.Interfaces;
using Spacearr.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Spacearr.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetStorageFolderPath()
        {
            return Plugin.XF.AppInstallHelper.CrossInstallHelper.Current.GetPublicDownloadPath();
        }
    }
}
