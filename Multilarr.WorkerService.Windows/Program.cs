using Microsoft.Extensions.Configuration;
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
                    services.AddSingleton<ILoggerDatabase, LoggerDatabase>(serviceProvider =>
                        new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrWorkerServiceSQLite.db3")));
                    services.AddSingleton<PusherServer.IPusherOptions, PusherServer.PusherOptions>(serviceProvider =>
                        new PusherServer.PusherOptions { Cluster = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherCluster").Value });
                    services.AddSingleton<PusherServer.IPusher, PusherServer.Pusher>(serviceProvider => new PusherServer.Pusher(
                        services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherAppId").Value,
                        services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherKey").Value,
                        services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherSecret").Value,
                        services.BuildServiceProvider().GetService<PusherServer.IPusherOptions>()));
                    services.AddSingleton<IPusherClientInterface, PusherClientInterface>(serviceProvider =>
                        new PusherClientInterface(services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherKey").Value,
                            new PusherClient.PusherOptions { Cluster = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("PusherCluster").Value }));

                    services.AddSingleton<IDataSize, DataSize>();
                    services.AddSingleton<IMultilarrMessageCommand, MultilarrMessageCommand>();
                    services.AddSingleton<IComputerDrives, ComputerDrives>();
                    services.AddSingleton<IComputerDriveInfo, ComputerDriveInfo>();
                    services.AddSingleton<ICommand, Common.Command.Command>();
                    services.AddSingleton<ILogger, Logger>();
                    services.AddSingleton<INotificationTimer, NotificationTimer>();
                    services.AddHostedService<Worker>();
                });
    }
}