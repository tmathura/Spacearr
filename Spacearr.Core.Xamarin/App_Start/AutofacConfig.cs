using Autofac;
using Spacearr.Common;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Logger;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using Spacearr.Pusher.API.Service;
using System;
using System.IO;

namespace Spacearr.Core.Xamarin
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spacearr.Core.Xamarin.SQLite.db3"))).As<ILogger>().SingleInstance();

            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<PusherValidation>().As<IPusherValidation>().SingleInstance();
            builder.RegisterType<Setting>().As<ISetting>().SingleInstance();
            builder.RegisterType<WorkerServiceReceiver>().As<IWorkerServiceReceiver>().SingleInstance();
            builder.RegisterType<ComputerDriveService>().As<IComputerDriveService>().SingleInstance();
        }
    }
}