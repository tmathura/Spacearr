using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Logger;
using Multilarr.WorkerService.Windows.Command;
using Multilarr.WorkerService.Windows.Command.MessageCommand;
using Multilarr.WorkerService.Windows.Common;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using System;
using System.IO;

namespace Multilarr.WorkerService.Windows
{
    public class Program
    {
        private const string AppId = "927757";
        private const string Key = "1989c6974272ea96b1c4";
        private const string Secret = "27dd35a15799cb4dac36";
        private const string Cluster = "ap2";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDataSize, DataSize>();
                    services.AddSingleton<IMultilarrMessageCommand, MultilarrMessageCommand>();
                    services.AddSingleton<IComputerDrives, ComputerDrives>();
                    services.AddSingleton<IComputerDriveInfo, ComputerDriveInfo>();
                    services.AddSingleton<ICommand, Command.Command>();
                    services.AddSingleton<ILoggerDatabase, LoggerDatabase>(serviceProvider => new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrWorkerServiceSQLite.db3")));
                    services.AddSingleton<ILogger, Logger>();
                    services.AddSingleton<PusherServer.IPusherOptions, PusherServer.PusherOptions>(serviceProvider => new PusherServer.PusherOptions { Cluster = Cluster });
                    services.AddSingleton<PusherServer.IPusher, PusherServer.Pusher>(serviceProvider => new PusherServer.Pusher(AppId, Key, Secret, services.BuildServiceProvider().GetService<PusherServer.IPusherOptions>()));
                    services.AddSingleton<INotificationTimer, NotificationTimer>(serviceProvider => new NotificationTimer(900000, services.BuildServiceProvider().GetService<ICommand>(), services.BuildServiceProvider().GetService<PusherServer.IPusher>()));
                    services.AddHostedService<Worker>();
                });
    }
}