using Autofac;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Logger;
using Spacearr.Core.Xamarin.Services.Implementations;
using Spacearr.Core.Xamarin.Services.Interfaces;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using Spacearr.Pusher.API.Service;
using System;
using System.IO;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spacearr.Core.Xamarin.SQLite.db3"))).As<ILogger>().SingleInstance();
            builder.Register(c => DependencyService.Get<IFileService>()).As<IFileService>().SingleInstance();
            builder.RegisterType<DownloadService>().As<IDownloadService>().SingleInstance();
            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<PusherValidation>().As<IPusherValidation>().SingleInstance();
            builder.RegisterType<WorkerServiceReceiver>().As<IWorkerServiceReceiver>().SingleInstance();
            builder.RegisterType<GetComputerService>().As<IGetComputerService>().SingleInstance();
            builder.RegisterType<SaveFirebasePushNotificationTokenService>().As<ISaveFirebasePushNotificationTokenService>().SingleInstance();
        }
    }
}