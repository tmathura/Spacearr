using Autofac;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Logger;
using Multilarr.Pusher.API;
using Multilarr.Pusher.API.Interfaces;
using Multilarr.Pusher.API.Interfaces.Service;
using Multilarr.Pusher.API.Service;
using System;
using System.IO;

namespace Multilarr.Core
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"))).As<ILogger>().SingleInstance();

            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<PusherValidation>().As<IPusherValidation>().SingleInstance();
            builder.RegisterType<Setting>().As<ISetting>().SingleInstance();
            builder.RegisterType<WorkerServiceReceiver>().As<IWorkerServiceReceiver>().SingleInstance();
            builder.RegisterType<ComputerDriveService>().As<IComputerDriveService>().SingleInstance();
        }
    }
}