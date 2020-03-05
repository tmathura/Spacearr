using Multilarr.Common;
using Multilarr.Common.Interfaces;
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
        private readonly ISetting _setting;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;

        public NotificationReceiver(ILogger logger, ISetting setting)
        {
            _logger = logger;
            _setting = setting;

           _channelNameReceive = Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString();
           _eventNameReceive = Enumeration.PusherEvent.WorkerServiceEvent.ToString();
        }

        public async Task Connect()
        {
            try
            {
                _setting.PopulateSetting();

                if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
                {
                    var pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });

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
