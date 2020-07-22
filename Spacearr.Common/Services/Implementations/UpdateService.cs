using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Octokit;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Common.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Implementations
{
    public class UpdateService : IUpdateService
    {
        /// <summary>
        /// The latest tag name for _latestRelease which is set when CheckForUpdateAsync is done.
        /// </summary>
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
        private readonly ILogger _logger;
        private readonly IGitHubClient _gitHubClient;
        private readonly IFileService _fileService;

        public UpdateService(ILogger logger, IGitHubClient gitHubClient, IFileService fileService)
        {
            _logger = logger;
            _gitHubClient = gitHubClient;
            _fileService = fileService;
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

        /// <summary>
        /// Get the update file name for a specific device type of the last CheckForUpdateAsync.
        /// </summary>
        /// <returns></returns>
        public async Task<string> FileNameOfLastUpdateCheck(UpdateType updateType)
        {
            try
            {
                if (_latestRelease == null)
                {
                    throw new Exception("Please do CheckForUpdateAsync first");
                }

                string fileName;
                switch (updateType)
                {
                    case UpdateType.Android:
                        fileName = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("apk"))?.Name;
                        break;
                    case UpdateType.WindowsService:
                        fileName = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("windowsservice"))?.Name;
                        break;
                    case UpdateType.WorkerService:
                        fileName = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("workerservice"))?.Name;
                        break;
                    case UpdateType.Uwp:
                        fileName = _latestRelease.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("uwp"))?.Name;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Do the update process.
        /// </summary>
        /// <param name="updateType"></param>
        /// <returns></returns>
        public async Task UpdateApp(UpdateType updateType)
        {
            var fileName = await FileNameOfLastUpdateCheck(updateType);
            await _fileService.ExtractFiles(fileName);
            await UpdateFiles(updateType);
            await _fileService.DeleteUpdateFolder();
        }

        /// <summary>
        /// Update the app with the new files.
        /// </summary>
        /// <param name="updateType"></param>
        /// <returns></returns>
        private async Task UpdateFiles(UpdateType updateType)
        {
            var appName = updateType == UpdateType.WorkerService ? "Spacearr.WorkerService.Windows" : "Spacearr.Windows.Windows";
            var updateFilesPath = Path.Combine(_fileService.GetUpdateAppStorageFolderPath(), $@"Spacearr\{appName}");
            var parentDirectory = Directory.GetParent(_fileService.GetUpdateAppStorageFolderPath()).Parent;
            var currentAppPath = Path.GetFullPath(parentDirectory?.FullName ?? string.Empty);

            foreach (var dirPath in Directory.GetDirectories(updateFilesPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(updateFilesPath, currentAppPath));
            }

            foreach (var filename in Directory.EnumerateFiles(updateFilesPath, "*", SearchOption.AllDirectories))
            {
                if (filename.ToLower().Contains("appsettings.json"))
                {
                    await UpdateConfigFile(filename, currentAppPath, updateFilesPath);
                }
                else
                {
                    await CopyFile(filename, currentAppPath, updateFilesPath);
                }
            }
        }

        /// <summary>
        /// Copy update files.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newPath"></param>
        /// <param name="oldParentPath"></param>
        /// <returns></returns>
        private static async Task CopyFile(string filePath, string newPath, string oldParentPath)
        {
            using (var sourceStream = File.Open(filePath, System.IO.FileMode.Open))
            {
                using (var destinationStream = File.Create(filePath.Replace(oldParentPath, newPath)))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
        }

        /// <summary>
        /// Update config file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newPath"></param>
        /// <param name="oldParentPath"></param>
        /// <returns></returns>
        private static async Task UpdateConfigFile(string filePath, string newPath, string oldParentPath)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("appsettings.json").Build();

            var appSettingsModel = new AppSettingsModel();

            configuration.Bind(appSettingsModel);

            var updatedJsonString =  await Task.FromResult(JsonConvert.SerializeObject(appSettingsModel, Formatting.Indented));
            File.WriteAllText(filePath.Replace(oldParentPath, newPath), updatedJsonString);
        }
    }
}
