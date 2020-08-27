using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Common.Timers.Interfaces;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Spacearr.Common.Timers.Implementations
{
    public class UpdateAppTimer : IUpdateAppTimer
    {
        private readonly IConfiguration _configuration;
        private readonly IInvoker _invoker;
        private readonly ILogger _logger;
        private readonly IUpdateService _updateService;
        private readonly IDownloadService _downloadService;
        private readonly IFileService _fileService;

        private readonly Timer _timer;

        public UpdateAppTimer(IConfiguration configuration, IInvoker invoker, ILogger logger, IUpdateService updateService, IDownloadService downloadService, IFileService fileService)
        {
            _configuration = configuration;
            _invoker = invoker;
            _logger = logger;
            _updateService = updateService;
            _downloadService = downloadService;
            _fileService = fileService;

            _timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(Convert.ToDouble(_configuration.GetSection("UpdateAppInterval").Value)).TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += ElapsedEventHandler;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Instantiate()
        {
            if (Convert.ToBoolean(_configuration.GetSection("AutoUpdateApp").Value))
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void DeInstantiate()
        {
            if (Convert.ToBoolean(_configuration.GetSection("AutoUpdateApp").Value))
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// The ElapsedEventHandler for the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_downloadService.IsDownloading)
                {
                    var command = new UpdateCommand(_logger, _updateService, _downloadService, _fileService);
                    await _invoker.Invoke(command);
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
            }
        }
    }
}