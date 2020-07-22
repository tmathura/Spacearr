using Microsoft.Extensions.Hosting;
using Spacearr.Common.Enums;
using Spacearr.Common.Timers.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.WorkerService.Windows.Updater
{
    public class Worker : BackgroundService
    {
        public Worker(IUpdateAppTimer updateAppTimer)
        {
            updateAppTimer.Instantiate(UpdateType.WorkerService);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
