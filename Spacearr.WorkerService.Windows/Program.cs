using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spacearr.Common.Command.Implementations;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Implementations;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Logger.Implementations;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Common.Timers.Implementations;
using Spacearr.Common.Timers.Interfaces;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Receivers.Implementations;
using Spacearr.Pusher.API.Receivers.Interfaces;
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
                    services.AddSingleton<ISendFirebasePushNotificationService, SendFirebasePushNotificationService>();
                    services.AddSingleton<ILowSpaceTimer, LowSpaceTimer>();
                    services.AddSingleton<IComputerDrivesCommandReceiver, ComputerDrivesCommandReceiver>();
                    services.AddSingleton<ISaveFirebasePushNotificationTokenCommandReceiver, SaveFirebasePushNotificationTokenCommandReceiver>();
                });
    }
}