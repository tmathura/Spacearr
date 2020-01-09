using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilarr.WorkerService.Windows.Command;
using Multilarr.WorkerService.Windows.Command.MessageCommand;
using Multilarr.WorkerService.Windows.Common;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using Multilarr.WorkerService.Windows.Common.Interfaces.Logger;
using Multilarr.WorkerService.Windows.Common.Logger;
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
                    services.AddSingleton<IDataSize, DataSize>();
                    services.AddSingleton<IMultilarrMessageCommand, MultilarrMessageCommand>();
                    services.AddSingleton<IComputerDrives, ComputerDrives>();
                    services.AddSingleton<IComputerDriveInfo, ComputerDriveInfo>();
                    services.AddSingleton<ICommand, Command.Command>();
                    services.AddSingleton<ILoggerDatabase, LoggerDatabase>(serviceProvider => new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrWorkerServiceSQLite.db3")));
                    services.AddSingleton<ILogger, Logger>();
                    services.AddHostedService<Worker>();
                });
    }
}