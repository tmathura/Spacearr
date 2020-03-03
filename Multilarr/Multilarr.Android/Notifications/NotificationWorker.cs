using Android.Content;
using AndroidX.Work;
using Multilarr.Common;
using Multilarr.Common.Models;
using Multilarr.Common.Pusher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Logger = Multilarr.Common.Logger.Logger;

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
            _logger = new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MultilarrSQLite.db3"));

            try
            {
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
                        _pusher[pusherCount].NotificationReceiverConnect();
                        pusherCount += 1;
                    }
                }
            }
            catch (Exception e)
            {
                _ = _logger.LogErrorAsync(e.Message, e.StackTrace);
            }

            _notificationManager = new AndroidNotificationManager();
        }

        public override Result DoWork()
        {
            var notifications = _logger.GetNotificationLogsAsync().Result;
            foreach (var notification in notifications)
            {
                _notificationManager.ScheduleNotification(notification.LogTitle, notification.LogMessage);
                _ = _logger.DeleteLogAsync(notification);
            }

            return Result.InvokeSuccess();
        }
    }
}