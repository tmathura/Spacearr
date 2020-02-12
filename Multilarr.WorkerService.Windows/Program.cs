using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilarr.Common;
using Multilarr.Common.Command;
using Multilarr.Common.Command.MessageCommand;
using Multilarr.Common.Interfaces;
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
                    services.AddSingleton<ILoggerDatabase, LoggerDatabase>(serviceProvider => new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrWorkerServiceSQLite.db3")));

                    services.AddHostedService<Worker>();
                    services.AddSingleton<ILogger, Logger>();
                    services.AddSingleton<IPusher, Pusher>();
                    services.AddSingleton<ICommand, Command>();
                    services.AddSingleton<IDataSize, DataSize>();
                    services.AddSingleton<IComputerDrives, ComputerDrives>();
                    services.AddSingleton<IComputerDriveInfo, ComputerDriveInfo>();
                    services.AddSingleton<INotificationTimer, NotificationTimer>();
                    services.AddSingleton<IMultilarrMessageCommand, MultilarrMessageCommand>();
                });
    }
}