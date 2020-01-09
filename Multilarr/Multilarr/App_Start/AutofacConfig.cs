using Autofac;
using Multilarr.Common.Logger;
using Multilarr.Services;
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

            var optionsSend = new PusherServer.PusherOptions { Cluster = Cluster };

            var optionsReceive = new PusherClient.PusherOptions { Cluster = Cluster };
            var pusherReceive = new PusherClient.Pusher(Key, optionsReceive);
            pusherReceive.ConnectAsync();

            builder.Register(c => new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"))).As<ILoggerDatabase>().SingleInstance();
            builder.Register(c => new Logger(c.Resolve<ILoggerDatabase>())).As<ILogger>().SingleInstance();
            builder.Register(c => new PusherServer.Pusher(AppId, Key, Secret, optionsSend)).As<PusherServer.IPusher>().SingleInstance();
            builder.Register(c => new ComputerDriveDataStore(c.Resolve<PusherServer.IPusher>(), pusherReceive, c.Resolve<ILogger>())).As<IComputerDriveDataStore>().SingleInstance();
        }
    }
}