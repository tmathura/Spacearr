using Autofac;
using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Implementations;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Implementations;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Logger.Implementations;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Receivers.Implementations;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.IO;

namespace Spacearr.Windows.Service
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spacearr.Windows.Service.SQLite.db3"))).As<ILogger>().SingleInstance();
            
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            builder.Register(c => configuration).As<IConfiguration>().SingleInstance();

            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<Invoker>().As<IInvoker>().SingleInstance();
            builder.RegisterType<ComputerDrives>().As<IComputerDrives>().SingleInstance();
            builder.RegisterType<ComputerDriveInfo>().As<IComputerDriveInfo>().SingleInstance();
            builder.RegisterType<SendFirebasePushNotificationService>().As<ISendFirebasePushNotificationService>().SingleInstance();
            builder.RegisterType<NotificationTimerService>().As<INotificationTimerService>().SingleInstance();
            builder.RegisterType<ComputerDrivesCommandReceiver>().As<IComputerDrivesCommandReceiver>().SingleInstance();
            builder.RegisterType<SaveFirebasePushNotificationTokenCommandReceiver>().As<ISaveFirebasePushNotificationTokenCommandReceiver>().SingleInstance();
        }
    }
}