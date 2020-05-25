using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spacearr.Common;
using Spacearr.Common.Command;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Logger;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.IO;

namespace Spacearr.WorkerService.Windows
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
                    services.AddSingleton<ILogger, Logger>(serviceProvider => new Logger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spacearr.WorkerService.Windows.SQLite.db3")));

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