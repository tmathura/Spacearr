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
        private const string AppName = "Spacearr.WorkerService.Windows";
        private readonly ILogger _logger;
        private readonly IUpdateService _updateService;
        private readonly IDownloadService _downloadService;
        private readonly IFileService _fileService;

        public UpdateCommand(ILogger logger, IUpdateService updateService, IDownloadService downloadService, IFileService fileService)
        {
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
            var parentDirectory = Directory.GetParent(_fileService.GetUpdateAppStorageFolderPath()).Parent;
            var currentAppPath = Path.GetFullPath(parentDirectory?.FullName ?? string.Empty);
            var currentApp = Path.Combine(currentAppPath, $"{AppName}.dll");

            var assemblyName = AssemblyName.GetAssemblyName(currentApp);

            if (await _updateService.CheckForUpdateAsync(assemblyName.Version.ToString()))
            {
                var url = await _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.WorkerService);
                var progressIndicator = new Progress<double>();
                var cts = new CancellationTokenSource();
                try
                {
                    await _fileService.DeleteUpdateFolder();
                    Directory.CreateDirectory(_fileService.GetUpdateAppStorageFolderPath());
                    await _downloadService.DownloadFileAsync(url, progressIndicator, cts.Token);

                    var controller = new ServiceController(AppName);

                    if (controller.Status != ServiceControllerStatus.StopPending && controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    }

                    await _updateService.UpdateApp(UpdateType.WorkerService);

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