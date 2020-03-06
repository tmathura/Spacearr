using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilarr.Common;
using Multilarr.Common.Command;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Logger;
using Multilarr.Pusher.API;
using Multilarr.Pusher.API.Interfaces;
using System;
using System.IO;

namespace Multilarr.WorkerService.Windows
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILogger, Logger>(serviceProvider => new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrWorkerServiceSQLite.db3")));

                    services.AddHostedService<Worker>();
                    services.AddSingleton<IPusher, Pusher.API.Pusher>();
                    services.AddSingleton<IInvoker, Invoker>();
                    services.AddSingleton<IComputerDrives, ComputerDrives>();
                    services.AddSingleton<IComputerDriveInfo, ComputerDriveInfo>();
                    services.AddSingleton<INotificationTimer, NotificationTimer>();
                    services.AddSingleton<IComputerDrivesCommandReceiver, ComputerDrivesCommandReceiver>();
                });
    }
}