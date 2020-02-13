using Android.Content;
using AndroidX.Work;
using Multilarr.Common;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Logger = Multilarr.Common.Logger;

namespace Multilarr.Droid.Notifications
{
    public class NotificationWorker : Worker
    {

        private readonly AndroidNotificationManager _notificationManager;
        private readonly List<Pusher> _pusher;
        private readonly Logger _logger;

        public NotificationWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            _pusher = new List<Pusher>();
            var loggerDatabase = new LoggerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"));
            _logger = new Logger(loggerDatabase);

            var settings = _logger.GetSettingLogsAsync().Result;

            var pusherCount = 0;
            foreach (var setting in settings)
            {
                var pusherSend = new PusherServer.Pusher(setting.PusherAppId, setting.PusherKey, setting.PusherSecret, new PusherServer.PusherOptions { Cluster = setting.PusherCluster });

                var getResult = pusherSend.GetAsync<object>($"/channels/{Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString()}").Result;
                var pusherSendRequestResultObject = JsonConvert.DeserializeObject<PusherSendRequestResultObject>(((PusherServer.RequestResult)getResult).Body);

                if (!pusherSendRequestResultObject.Occupied)
                {
                    _pusher.Add(new Pusher(_logger, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster));
                    _ = _pusher[pusherCount].NotificationReceiverConnect(Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString(), Enumeration.PusherEvent.WorkerServiceEvent.ToString());
                    pusherCount += 1;
                }
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
    }
}