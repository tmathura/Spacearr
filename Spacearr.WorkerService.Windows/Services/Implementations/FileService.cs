using System;
using System.IO;
using System.Threading.Tasks;
using Spacearr.Common.Services.Interfaces;

namespace Spacearr.WorkerService.Windows.Services.Implementations
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
