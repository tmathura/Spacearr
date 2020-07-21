using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class UpdateCommand : ICommand
    {
        private readonly UpdateType _updateType;
        private readonly ILogger _logger;
        private readonly IUpdateService _updateService;
        private readonly IDownloadService _downloadService;
        private readonly IFileService _fileService;

        public UpdateCommand(UpdateType updateType, ILogger logger, IUpdateService updateService, IDownloadService downloadService, IFileService fileService)
        {
            _updateType = updateType;
            _logger = logger;
            _updateService = updateService;
            _downloadService = downloadService;
            _fileService = fileService;
        }

        /// <summary>
        /// Update app.
        /// </summary>
        /// <returns>string.Empty</returns>
        public async Task<string> Execute()
        {
            var appName = _updateType == UpdateType.WorkerService ? "Spacearr.WorkerService.Windows" : "Spacearr.Windows.Windows";
            var parentDirectory = Directory.GetParent(_fileService.GetUpdateAppStorageFolderPath()).Parent;
            var currentAppPath = Path.GetFullPath(parentDirectory?.FullName ?? string.Empty);
            var currentApp = Path.Combine(currentAppPath, $"{appName}.dll");

            var assemblyName = AssemblyName.GetAssemblyName(currentApp);

            if (await _updateService.CheckForUpdateAsync(assemblyName.Version.ToString()))
            {
                var url = await _updateService.UpdateUrlOfLastUpdateCheck(_updateType);
                var progressIndicator = new Progress<double>();
                var cts = new CancellationTokenSource();
                try
                {
                    await _fileService.DeleteUpdateFolder();
                    Directory.CreateDirectory(_fileService.GetUpdateAppStorageFolderPath());
                    await _downloadService.DownloadFileAsync(url, progressIndicator, cts.Token);

                    var controller = new ServiceController(appName);

                    if (controller.Status != ServiceControllerStatus.StopPending && controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    }

                    await _updateService.UpdateApp(_updateType);

                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (OperationCanceledException ex)
                {
                    await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                }
            }
            return string.Empty;
        }
    }
}