using Android.Content;
using AndroidX.Work;
using Multilarr.Common;
using Multilarr.Common.Logger;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Logger = Multilarr.Common.Logger.Logger;

namespace Multilarr.Droid.Notifications
{
    public class NotificationWorker : Worker
    {
        private const string AppId = "927757";
        private const string Key = "1989c6974272ea96b1c4";
        private const string Secret = "27dd35a15799cb4dac36";
        private const string Cluster = "ap2";

        private readonly AndroidNotificationManager _notificationManager;
        private readonly PusherServer.Pusher _pusherSend;
        private readonly PusherClientInterface _pusherReceive;
        private PusherClient.Channel _myChannel;
        private readonly Logger _logger;

        public NotificationWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            var loggerDatabase = new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"));
            _logger = new Logger(loggerDatabase);

            _pusherSend = new PusherServer.Pusher(AppId, Key, Secret, new PusherServer.PusherOptions { Cluster = Cluster });

            var getResult = _pusherSend.GetAsync<object>("/channels/multilarr-worker-service-windows-notification-channel").Result;
            var pusherSendRequestResultObject = JsonConvert.DeserializeObject<PusherSendRequestResultObject>(((PusherServer.RequestResult) getResult).Body);

            if (!pusherSendRequestResultObject.Occupied)
            {
                _pusherReceive = new PusherClientInterface(Key, new PusherClient.PusherOptions { Cluster = Cluster });
                _pusherReceive.ConnectAsync();

                _ = SubscribeChannel();
            }

            _notificationManager = new AndroidNotificationManager();
        }

        public override Result DoWork()
        {
            var notifications = _logger.GetNotificationLogsAsync().Result;
            foreach (var notification in notifications)
            {
                _notificationManager.ScheduleNotification(notification.LogTitle, notification.LogMessage);
               var deletedResult =  _logger.DeleteLogAsync(notification).Result;
            }

            return Result.InvokeSuccess();
        }

        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-worker-service-windows-notification-channel");
            _myChannel.Bind("worker_service_event", (dynamic data) =>
            {
                var logs = _logger.GetNotificationLogsAsync().Result;

                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgs>(pusherReceiveMessage.Message);

                _logger.LogNotificationAsync(deserializeObject.Title, deserializeObject.Message);
            });
        }
    }
}