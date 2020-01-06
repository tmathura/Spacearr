using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Multilarr.WorkerService.Windows.Command;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICommand _command;

        public Worker(ILogger<Worker> logger, ICommand command)
        {
            _logger = logger;
            _command = command;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var commandObjectSerialized = _command.Invoke("drives");
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
