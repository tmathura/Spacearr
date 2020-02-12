using Microsoft.Extensions.Hosting;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        public Worker(IPusher pusher, INotificationTimer notificationTimer)
        {
            pusher.CommandReceiverConnect(
                Enumeration.PusherChannel.MultilarrChannel.ToString(),
                Enumeration.PusherEvent.MultilarrEvent.ToString(),
                Enumeration.PusherChannel.MultilarrWorkerServiceWindowsChannel.ToString(),
                Enumeration.PusherEvent.WorkerServiceEvent.ToString()
                );

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