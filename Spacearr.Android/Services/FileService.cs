using Spacearr.Common.Services.Interfaces;
using Spacearr.Droid.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Spacearr.Droid.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Get the update folder path.
        /// </summary>
        /// <returns>update path</returns>
        public string GetUpdateAppStorageFolderPath()
        {
            return Plugin.XF.AppInstallHelper.CrossInstallHelper.Current.GetPublicDownloadPath();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <returns></returns>
        public Task DeleteUpdateFolder()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <returns></returns>
        public Task ExtractFiles(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
