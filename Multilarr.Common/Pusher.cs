using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Multilarr.Common
{
    public class Pusher : IPusher
    {
        private readonly ILogger _logger;
        private readonly ICommand _command;
        private readonly ISetting _setting;

        private PusherClient.Channel _myChannel;
        private PusherClient.Pusher _pusherReceive;
        public string ReturnData { get; set; }

        public Pusher(ILogger logger, ICommand command, IConfiguration configuration)
        {
            var setting = new Setting
            {
                AppId = configuration.GetSection("PusherAppId").Value,
                Key = configuration.GetSection("PusherKey").Value,
                Secret = configuration.GetSection("PusherSecret").Value,
                Cluster = configuration.GetSection("PusherCluster").Value
            };

            _logger = logger;
            _command = command;
            _setting = setting;
        }

        public Pusher(ILogger logger, ISetting setting)
        {
            _logger = logger;
            _command = null;
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

        public async Task CommandReceiverConnect(string channelNameReceive, string eventNameReceive, string channelNameSend, string eventNameSend)
        {
            _pusherReceive = null;
            ReturnData = null;
            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                _pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });
            }

            _myChannel = await _pusherReceive.SubscribeAsync(channelNameReceive);
            _myChannel.Bind(eventNameReceive, (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessage.Data);
                var deserializeObject = JsonConvert.DeserializeObject<PusherSendMessage>(pusherMessage.Message);
                ExecuteCommand(deserializeObject.Command, channelNameSend, eventNameSend);
            });

            await _pusherReceive.ConnectAsync();
        }

        public async Task ReceiverDisconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }

        private async void ExecuteCommand(Enumeration.CommandType command, string channelName, string eventName)
        {
            try
            {
                var commandObjectSerialized = _command.Invoke(command);
                await SendMessage(channelName, eventName, commandObjectSerialized.SerializeObject);
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message);
            }
        }
    }
}
