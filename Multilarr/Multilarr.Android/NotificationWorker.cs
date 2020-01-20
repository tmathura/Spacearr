using Android.Content;
using AndroidX.Work;
using Multilarr.Common.Models;
using Multilarr.Common.NotificationLogger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Multilarr.Droid
{
    public class NotificationWorker : Worker
    {
        private const string Key = "1989c6974272ea96b1c4";
        private const string Cluster = "ap2";

        private readonly AndroidNotificationManager _notificationManager;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        private readonly NotificationLogger _notificationLogger;

        public NotificationWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            var optionsReceive = new PusherClient.PusherOptions { Cluster = Cluster };
            _pusherReceive = new PusherClient.Pusher(Key, optionsReceive);
            _pusherReceive.ConnectAsync();

            _ = SubscribeChannel();

            _notificationManager = new AndroidNotificationManager();

            var notificationLoggerDatabase = new NotificationLoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"));
            _notificationLogger = new NotificationLogger(notificationLoggerDatabase);
        }

        public override Result DoWork()
        {
            var notifications = _notificationLogger.GetLogsAsync().Result;
            foreach (var notification in notifications)
            {
                _notificationManager.ScheduleNotification(notification.LogTitle, notification.LogMessage);
               var deletedResult =  _notificationLogger.DeleteLogAsync(notification).Result;
            }

            return Result.InvokeSuccess();
        }

        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-worker-service-windows-notification-channel");
            _myChannel.Bind("worker_service_event", (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgs>(pusherReceiveMessage.Message);

                _notificationLogger.LogNotificationAsync(deserializeObject.Title, deserializeObject.Message);
            });
        }
    }
}