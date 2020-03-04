using Autofac;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Interfaces.Pusher;
using Multilarr.Common.Logger;
using Multilarr.Common.Pusher;
using Multilarr.Pusher.API;
using Multilarr.Pusher.API.Interfaces;
using System;
using System.IO;

namespace Multilarr
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"))).As<ILogger>().SingleInstance();

            builder.RegisterType<Common.Pusher.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<Setting>().As<ISetting>().SingleInstance();
            builder.RegisterType<ServiceReceiverConnect>().As<IServiceReceiverConnect>().SingleInstance();
            builder.RegisterType<ComputerDriveService>().As<IComputerDriveService>().SingleInstance();
        }
    }
}