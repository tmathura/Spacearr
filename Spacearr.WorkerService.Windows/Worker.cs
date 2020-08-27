using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Spacearr.Common.Timers.Interfaces;
using Spacearr.Pusher.API;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        public Worker(IConfiguration configuration, IPusher pusher, ILowSpaceTimer lowSpaceTimer)
        {
            pusher.GetComputerDrivesCommandReceiverConnect(configuration.GetSection("PusherAppId").Value, configuration.GetSection("PusherKey").Value,
                configuration.GetSection("PusherSecret").Value, configuration.GetSection("PusherCluster").Value);
            pusher.SaveFirebasePushNotificationTokenCommandReceiverConnect(configuration.GetSection("PusherAppId").Value, configuration.GetSection("PusherKey").Value,
                configuration.GetSection("PusherSecret").Value, configuration.GetSection("PusherCluster").Value);
            lowSpaceTimer.Instantiate();
            pusher.GetWorkerServiceVersionCommandReceiverConnect(configuration.GetSection("PusherAppId").Value, configuration.GetSection("PusherKey").Value,
                configuration.GetSection("PusherSecret").Value, configuration.GetSection("PusherCluster").Value);
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