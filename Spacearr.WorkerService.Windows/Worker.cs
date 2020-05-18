using Microsoft.Extensions.Hosting;
using Spacearr.Common.Interfaces;
using Spacearr.Pusher.API.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        public Worker(IPusher pusher, INotificationTimer notificationTimer)
        {
            pusher.ComputerDrivesCommandReceiverConnect();
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