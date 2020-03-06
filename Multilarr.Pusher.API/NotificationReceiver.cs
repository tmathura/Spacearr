﻿using Multilarr.Common;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Pusher.API.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API
{
    public class NotificationReceiver : INotificationReceiver
    {
        private readonly ILogger _logger;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;

        public NotificationReceiver(ILogger logger)
        {
            _logger = logger;

           _channelNameReceive = Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString();
           _eventNameReceive = Enumeration.PusherEvent.WorkerServiceEvent.ToString();
        }

        public async Task Connect(string appId, string key, string secret, string cluster)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(appId) && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret) && !string.IsNullOrWhiteSpace(cluster))
                {
                    var pusherReceive = new PusherClient.Pusher(key, new PusherClient.PusherOptions { Cluster = cluster });

                    var myChannel = await pusherReceive.SubscribeAsync(_channelNameReceive);
                    myChannel.Bind(_eventNameReceive, (dynamic data) =>
                    {
                        PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                        var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                        var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgs>(pusherReceiveMessage.Message);

                        _logger.LogNotificationAsync(deserializeObject.Title, deserializeObject.Message);
                    });

                    await pusherReceive.ConnectAsync();
                }
                else
                {
                    throw new Exception("No default setting saved.");
                }
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message, e.StackTrace);
            }
        }
    }
}
