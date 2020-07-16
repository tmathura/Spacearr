using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Package;
using Spacearr.Common.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Spacearr.WorkerService.Windows.Updater.Services.Implementations
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Get the update folder path.
        /// </summary>
        /// <returns>update path</returns>
        public string GetUpdateAppStorageFolderPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Update");
        }

        /// <summary>
        /// Delete the update folder path.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteUpdateFolder()
        {
            if (Directory.Exists(GetUpdateAppStorageFolderPath()))
            {
                await Task.Run(() => { Directory.Delete(GetUpdateAppStorageFolderPath(), true); });
            }
        }

        /// <summary>
        /// Extract the update files which are in the .msi package.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task ExtractFiles(string fileName)
        {
            var filePath = Path.Combine(GetUpdateAppStorageFolderPath(), fileName);

            await Task.Run(() =>
            {
                using var package = new InstallPackage(filePath, DatabaseOpenMode.ReadOnly);
                package.ExtractFiles();
            });
        }
    }
}
