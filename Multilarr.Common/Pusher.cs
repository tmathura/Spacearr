using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Multilarr.Common
{
    public class Pusher : IPusher
    {
        private readonly ISetting _setting;

        private PusherClient.Channel _myChannel;
        private PusherClient.Pusher _pusherReceive;
        public string ReturnData { get; set; }

        public Pusher(ISetting setting)
        {
            _setting = setting;
        }

        public async Task SendMessage(string channelName, string eventName, string message)
        {
            PusherServer.Pusher pusherSend = null;
            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                pusherSend = new PusherServer.Pusher(_setting.AppId, _setting.Key, _setting.Secret, new PusherServer.PusherOptions { Cluster = _setting.Cluster });
            }

            await pusherSend.TriggerAsync(channelName, eventName, new { message });
        }

        public async Task ReceiverConnect(string channelName, string eventName)
        {
            _pusherReceive = null;
            ReturnData = null;
            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                _pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });
            }

            _myChannel = await _pusherReceive.SubscribeAsync(channelName);
            _myChannel.Bind(eventName, (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                ReturnData = pusherReceiveMessage.Message;
            });

            await _pusherReceive.ConnectAsync();
        }

        public async Task ReceiverDisconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }
    }
}
