using Newtonsoft.Json;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class WorkerServiceReceiver : IWorkerServiceReceiver
    {
        private readonly ILogger _logger;
        public string ReturnData { get; set; }
        private PusherClient.Pusher _pusherReceive;

        public WorkerServiceReceiver(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Connect(string channelNameReceive, string eventNameReceive, string appId, string key, string secret, string cluster)
        {
            try
            {
                ReturnData = null;
                _pusherReceive = null;

                if (!string.IsNullOrWhiteSpace(appId) && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret) && !string.IsNullOrWhiteSpace(cluster))
                {
                    _pusherReceive = new PusherClient.Pusher(key, new PusherClient.PusherOptions { Cluster = cluster });

                    var myChannel = await _pusherReceive.SubscribeAsync(channelNameReceive);
                    myChannel.Bind(eventNameReceive, (dynamic data) =>
                    {
                        PusherReceiveMessageObjectModel pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObjectModel>(data.ToString());
                        var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageModel>(pusherReceiveMessageObject.Data);
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

        public async Task Disconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }
    }
}
