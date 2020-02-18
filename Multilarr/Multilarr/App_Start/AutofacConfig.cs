using Autofac;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Interfaces.Pusher;
using Multilarr.Common.Logger;
using Multilarr.Common.Pusher;
using Multilarr.Services;
using Multilarr.Services.Interfaces;
using System;
using System.IO;

namespace Multilarr
{
    public class AutofacConfig
    {
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