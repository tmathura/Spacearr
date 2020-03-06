using Microsoft.Extensions.Configuration;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API
{
    public class Pusher : IPusher
    {
        private readonly ILogger _logger;
        private readonly IInvoker _invoker;
        private readonly ISetting _setting;
        private readonly INotificationReceiver _notificationReceiver;
        private readonly IWorkerServiceReceiver _workerServiceReceiver;
        private readonly IComputerDrivesCommandReceiver _computerDrivesCommandReceiver;
        public string ReturnData => _workerServiceReceiver?.ReturnData;

        public Pusher(ILogger logger, ISetting setting, IWorkerServiceReceiver workerServiceReceiver)
        {
            _logger = logger;
            _setting = setting;
            _workerServiceReceiver = workerServiceReceiver;
        }

        public Pusher(ILogger logger, IConfiguration configuration, IInvoker invoker, IComputerDrivesCommandReceiver computerDrivesCommandReceiver)
        {
            _logger = logger;
            _invoker = invoker;
            _setting = new Setting(logger, configuration);
            _computerDrivesCommandReceiver = computerDrivesCommandReceiver;
            _notificationReceiver = new NotificationReceiver(_logger);
        }

        public async Task ComputerDrivesCommandReceiverConnect()
        {
            await _setting.PopulateSetting();
            await _computerDrivesCommandReceiver.Connect(ExecuteCommand, _setting.AppId, _setting.Key, _setting.Secret, _setting.Cluster);
        }

        public async Task WorkerServiceReceiverConnect(string channelName, string eventName)
        {
            await _setting.PopulateSetting();
            await _workerServiceReceiver.Connect(channelName, eventName, _setting.AppId, _setting.Key, _setting.Secret, _setting.Cluster);
        }

        public async Task WorkerServiceReceiverDisconnect()
        {
            await _workerServiceReceiver.Disconnect();
        }

        public async Task NotificationReceiverConnect()
        {
            await _setting.PopulateSetting();
            await _notificationReceiver.Connect(_setting.AppId, _setting.Key, _setting.Secret, _setting.Cluster);
        }

        public async Task SendMessage(string channelName, string eventName, string message)
        {
            await _setting.PopulateSetting();

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

        public static async Task<bool> Validate(string appId, string key, string secret, string cluster)
        {
            var pusherSend = new PusherServer.Pusher(appId, key, secret, new PusherServer.PusherOptions { Cluster = cluster });
            var getResult = await pusherSend.GetAsync<object>("/channels/ValidationTestChannel");
            return getResult.Data != null;
        }
    }
}
