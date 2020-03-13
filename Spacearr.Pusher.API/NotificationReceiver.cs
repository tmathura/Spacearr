using Newtonsoft.Json;
using Spacearr.Common;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class NotificationReceiver : INotificationReceiver
    {
        private readonly ILogger _logger;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;

        public NotificationReceiver(ILogger logger)
        {
            _logger = logger;

           _channelNameReceive = Enumeration.PusherChannel.SpacearrWorkerServiceWindowsNotificationChannel.ToString();
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
                        PusherReceiveMessageObjectModel pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObjectModel>(data.ToString());
                        var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageModel>(pusherReceiveMessageObject.Data);
                        var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgsModel>(pusherReceiveMessage.Message);

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
