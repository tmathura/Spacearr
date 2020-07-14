using Autofac;
using Octokit;
using Spacearr.Common.Logger.Implementations;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Pusher.API;
using Spacearr.Pusher.API.Receivers.Implementations;
using Spacearr.Pusher.API.Receivers.Interfaces;
using Spacearr.Pusher.API.Services.Implementations;
using Spacearr.Pusher.API.Services.Interfaces;
using Spacearr.Pusher.API.Validator.Implementations;
using Spacearr.Pusher.API.Validator.Interfaces;
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
            builder.Register(c => new GitHubClient(new ProductHeaderValue("Spacearr"))).As<IGitHubClient>().SingleInstance();
            builder.RegisterType<UpdateService>().As<IUpdateService>().SingleInstance();
            builder.RegisterType<DownloadService>().As<IDownloadService>().SingleInstance();
            builder.RegisterType<Pusher.API.Pusher>().As<IPusher>().SingleInstance();
            builder.RegisterType<PusherValidation>().As<IPusherValidation>().SingleInstance();
            builder.RegisterType<WorkerServiceReceiver>().As<IWorkerServiceReceiver>().SingleInstance();
            builder.RegisterType<GetComputerService>().As<IGetComputerService>().SingleInstance();
            builder.RegisterType<SaveFirebasePushNotificationTokenService>().As<ISaveFirebasePushNotificationTokenService>().SingleInstance();
        }
    }
}