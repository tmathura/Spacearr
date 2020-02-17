using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Multilarr.Common
{
    public class Pusher : IPusher
    {
        private readonly ILogger _logger;
        private readonly IInvoker _invoker;
        private readonly ISetting _setting;
        private readonly IComputerDrivesCommandReceiver _computerDrivesCommandReceiver;

        private PusherClient.Channel _myChannel;
        private PusherClient.Pusher _pusherReceive;
        public string ReturnData { get; set; }
        
        public Pusher(ILogger logger, ISetting setting)
        {
            _logger = logger;
            _setting = setting;
        }

        public Pusher(ILogger logger, IInvoker invoker, ISetting setting, IComputerDrivesCommandReceiver computerDrivesCommandReceiver)
        {
            _logger = logger;
            _invoker = invoker;
            _setting = setting;
            _computerDrivesCommandReceiver = computerDrivesCommandReceiver;
        }

        public Pusher(ILogger logger, string appId, string key, string secret, string cluster)
        {
            var setting = new Setting
            {
                AppId = appId,
                Key = key,
                Secret = secret,
                Cluster = cluster
            };

            _logger = logger;
            _setting = setting;
        }

        public void Connect()
        {
            _computerDrivesCommandReceiver.Connect(ExecuteCommand);
        }

        public async Task SendMessage(string channelName, string eventName, string message)
        {
            _setting.PopulateSetting();

            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                var pusherSend = new PusherServer.Pusher(_setting.AppId, _setting.Key, _setting.Secret, new PusherServer.PusherOptions { Cluster = _setting.Cluster });

                await pusherSend.TriggerAsync(channelName, eventName, new { message });
            }
            else
            {
                throw new Exception("No default setting saved.");
            }
        }

        public async Task ReceiverConnect(string channelName, string eventName)
        {
            _setting.PopulateSetting();
            _pusherReceive = null;
            ReturnData = null;

            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                _pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });

                _myChannel = await _pusherReceive.SubscribeAsync(channelName);
                _myChannel.Bind(eventName, (dynamic data) =>
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

        public async Task ReceiverDisconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }

        public async Task NotificationReceiverConnect(string channelNameReceive, string eventNameReceive)
        {
            _setting.PopulateSetting();
            _pusherReceive = null;
            ReturnData = null;

            if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
            {
                _pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });

                _myChannel = await _pusherReceive.SubscribeAsync(channelNameReceive);
                _myChannel.Bind(eventNameReceive, (dynamic data) =>
                {
                    var logs = _logger.GetNotificationLogsAsync().Result;

                    PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                    var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                    var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgs>(pusherReceiveMessage.Message);

                    _logger.LogNotificationAsync(deserializeObject.Title, deserializeObject.Message);
                });

                await _pusherReceive.ConnectAsync();
            }
            else
            {
                throw new Exception("No default setting saved.");
            }
        }

        public async void ExecuteCommand(ICommand command, string channelName, string eventName)
        {
            try
            {
                var json = _invoker.Invoke(command);
                await SendMessage(channelName, eventName, json);
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message, e.StackTrace);
            }
        }
    }
}
