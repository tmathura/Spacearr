using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Interfaces.Pusher;
using System;
using System.Threading.Tasks;

namespace Multilarr.Common.Pusher
{
    public class Pusher : IPusher
    {
        private readonly ILogger _logger;
        private readonly IInvoker _invoker;
        private readonly ISetting _setting;
        private readonly INotificationReceiver _notificationReceiver;
        private readonly IServiceReceiverConnect _serviceReceiverConnect;
        private readonly IComputerDrivesCommandReceiver _computerDrivesCommandReceiver;
        public string ReturnData => _serviceReceiverConnect?.ReturnData;

        public Pusher(ILogger logger, ISetting setting, IServiceReceiverConnect serviceReceiverConnect)
        {
            _logger = logger;
            _setting = setting;
            _serviceReceiverConnect = serviceReceiverConnect;
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
            _notificationReceiver = new NotificationReceiver(_logger, _setting);
        }

        public void CommandReceiverConnect()
        {
            _computerDrivesCommandReceiver.Connect(ExecuteCommand);
        }

        public void ServiceReceiverConnect(string channelName, string eventName)
        {
            _serviceReceiverConnect.Connect(channelName, eventName);
        }

        public void ServiceReceiverDisconnect()
        {
            _serviceReceiverConnect.ReceiverDisconnect();
        }

        public void NotificationReceiverConnect()
        {
            _notificationReceiver.Connect();
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
