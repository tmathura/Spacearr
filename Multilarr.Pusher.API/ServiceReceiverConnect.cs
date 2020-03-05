using System;
using System.Threading.Tasks;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Pusher.API.Interfaces;
using Newtonsoft.Json;

namespace Multilarr.Pusher.API
{
    public class ServiceReceiverConnect : IServiceReceiverConnect
    {
        private readonly ILogger _logger;
        private readonly ISetting _setting;
        public string ReturnData { get; set; }
        private PusherClient.Pusher _pusherReceive;

        public ServiceReceiverConnect(ILogger logger, ISetting setting)
        {
            _logger = logger;
            _setting = setting;
        }

        public async Task Connect(string channelNameReceive, string eventNameReceive)
        {
            try
            {
                ReturnData = null;
                _pusherReceive = null;
                _setting.PopulateSetting();

                if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
                {
                    _pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });

                    var myChannel = await _pusherReceive.SubscribeAsync(channelNameReceive);
                    myChannel.Bind(eventNameReceive, (dynamic data) =>
                    {
                        PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                        var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                        ReturnData = pusherReceiveMessage.Message;
                    });

                    await _pusherReceive.ConnectAsync();
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

        public async Task ReceiverDisconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }
    }
}
