using Autofac;
using Microsoft.Extensions.Configuration;
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

namespace Spacearr.Windows.Service
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpacearrSQLite.db3"))).As<ILogger>().SingleInstance();
            
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            builder.Register(c => configuration).As<IConfiguration>().SingleInstance();

            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<Invoker>().As<IInvoker>().SingleInstance();
            builder.RegisterType<ComputerDrives>().As<IComputerDrives>().SingleInstance();
            builder.RegisterType<ComputerDriveInfo>().As<IComputerDriveInfo>().SingleInstance();
            builder.RegisterType<NotificationTimer>().As<INotificationTimer>().SingleInstance();
            builder.RegisterType<ComputerDrivesCommandReceiver>().As<IComputerDrivesCommandReceiver>().SingleInstance();
        }
    }
}