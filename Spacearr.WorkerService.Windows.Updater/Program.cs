using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;
using Spacearr.Common.Command.Implementations;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Logger.Implementations;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Common.Timers.Implementations;
using Spacearr.Common.Timers.Interfaces;
using Spacearr.WorkerService.Windows.Updater.Services.Implementations;
using System;
using System.Diagnostics;
using System.IO;

namespace Spacearr.WorkerService.Windows.Updater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                using var eventLog = new EventLog("Application") { Source = "Spacearr Worker Service Windows Updater" };
                eventLog.WriteEntry($"{ex}", EventLogEntryType.Error);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILogger, Logger>(serviceProvider => new Logger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spacearr.WorkerService.Windows.Updater.SQLite.db3")));

                    services.AddHostedService<Worker>();
                    services.AddSingleton<IFileService, FileService>();
                    services.AddSingleton<IGitHubClient, GitHubClient>(serviceProvider => new GitHubClient(new ProductHeaderValue("Spacearr")));
                    services.AddSingleton<IUpdateService, UpdateService>();
                    services.AddSingleton<IDownloadService, DownloadService>();
                    services.AddSingleton<IInvoker, Invoker>();
                    services.AddSingleton<IUpdateAppTimer, UpdateAppTimer>();
                });
    }
}