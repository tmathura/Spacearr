using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Get the update folder path.
        /// </summary>
        /// <returns>update path</returns>
        string GetUpdateAppStorageFolderPath();

        /// <summary>
        /// Delete the update folder path.
        /// </summary>
        /// <returns></returns>
        Task DeleteUpdateFolder();

        /// <summary>
        /// Extract the update files which are in the .msi package.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task ExtractFiles(string fileName);
    }
}
