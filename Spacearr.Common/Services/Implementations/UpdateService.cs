using Octokit;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Implementations
{
    public class UpdateService : IUpdateService
    {
        public string LatestTagName
        {
            get
            {
                if (_latestRelease == null)
                {
                    throw new Exception("Please do CheckForUpdateAsync first");
                }

                return _latestRelease.TagName;
            }
        }
        private Release _latestRelease;
        private static ILogger _logger;
        private static IGitHubClient _gitHubClient;

        public UpdateService(ILogger logger, IGitHubClient gitHubClient)
        {
            _logger = logger;
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Check for an update for a specific version of the app on GitHub.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckForUpdateAsync(string version)
        {
            try
            {
                var releases = await _gitHubClient.Repository.Release.GetAll("tmathura", "Spacearr");
                _latestRelease = releases[0];
                var currentVersion = new Version(version);
                var latestVersion = new Version(_latestRelease.TagName);

                return latestVersion > currentVersion;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get update url for a specific device type of the last CheckForUpdateAsync.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateUrlOfLastUpdateCheck(UpdateType updateType)
        {
            try
            {
                if (_latestRelease == null)
                {
                    throw new Exception("Please do CheckForUpdateAsync first");
                }

                string url;
                switch (updateType)
                {
                    case UpdateType.Android:
                        url = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("apk"))?.BrowserDownloadUrl;
                        break;
                    case UpdateType.WindowsService:
                        url = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("windowsservice"))?.BrowserDownloadUrl;
                        break;
                    case UpdateType.WorkerService:
                        url = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("workerservice"))?.BrowserDownloadUrl;
                        break;
                    case UpdateType.Uwp:
                        url = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("uwp"))?.BrowserDownloadUrl;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
                }
                return url;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
