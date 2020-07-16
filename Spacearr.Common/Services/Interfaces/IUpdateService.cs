using Spacearr.Common.Enums;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IUpdateService
    {
        string LatestTagName { get; }

        /// <summary>
        /// Check for an update for a specific version of the app on GitHub.
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckForUpdateAsync(string version);

        /// <summary>
        /// Get update url for a specific device type of the last CheckForUpdateAsync.
        /// </summary>
        /// <returns></returns>
        Task<string> UpdateUrlOfLastUpdateCheck(UpdateType updateType);

        /// <summary>
        /// Get the update file name for a specific device type of the last CheckForUpdateAsync.
        /// </summary>
        /// <returns></returns>
        Task<string> FileNameOfLastUpdateCheck(UpdateType updateType);
        
        /// <summary>
        /// Do the update process.
        /// </summary>
        /// <param name="updateType"></param>
        /// <returns></returns>
        Task UpdateApp(UpdateType updateType);
    }
}