using Microsoft.Extensions.Hosting;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Pusher;
using System.Threading;
using System.Threading.Tasks;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        public Worker(IPusher pusher, INotificationTimer notificationTimer)
        {
            pusher.Connect();

            notificationTimer.Instantiate();
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