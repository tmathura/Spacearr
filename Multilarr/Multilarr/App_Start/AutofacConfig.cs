using Autofac;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Services;
using Multilarr.Services.Interfaces;
using System;
using System.IO;

namespace Multilarr
{
    public class AutofacConfig
    {
        private const string AppId = "927757";
        private const string Key = "1989c6974272ea96b1c4";
        private const string Secret = "27dd35a15799cb4dac36";
        private const string Cluster = "ap2";

        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"))).As<ILoggerDatabase>().SingleInstance();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<Setting>().As<ISetting>().SingleInstance();
            builder.RegisterType<Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<ComputerDriveService>().As<IComputerDriveService>().SingleInstance();
        }
    }
}