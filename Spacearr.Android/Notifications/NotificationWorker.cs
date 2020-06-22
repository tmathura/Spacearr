using Android.Content;
using AndroidX.Work;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Models;
using Spacearr.Pusher.API;
using System;
using System.Collections.Generic;
using System.IO;
using Logger = Spacearr.Common.Logger.Logger;

namespace Spacearr.Droid.Notifications
{
    public class NotificationWorker : Worker
    {

        private readonly AndroidNotificationManager _notificationManager;
        private readonly List<Pusher.API.Pusher> _pusher;
        private readonly Logger _logger;

        public NotificationWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            _pusher = new List<Pusher.API.Pusher>();
            _logger = new Logger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spacearr.Core.Xamarin.SQLite.db3"));

            try
            {
                var settings = _logger.GetSettingsAsync().Result;

                var pusherCount = 0;
                foreach (var setting in settings)
                {
                    var pusherSend = new PusherServer.Pusher(setting.PusherAppId, setting.PusherKey, setting.PusherSecret, new PusherServer.PusherOptions { Cluster = setting.PusherCluster });

                    var getResult = pusherSend.GetAsync<object>($"/channels/{PusherChannel.SpacearrWorkerServiceWindowsNotificationChannel}").Result;
                    var pusherSendRequestResultObject = JsonConvert.DeserializeObject<PusherSendRequestResultObjectModel>(((PusherServer.RequestResult)getResult).Body);

                    if (!pusherSendRequestResultObject.Occupied)
                    {
                        _pusher.Add(new Pusher.API.Pusher(_logger, new NotificationReceiver(_logger)));
                        _ = _pusher[pusherCount].NotificationReceiverConnect(setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);
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