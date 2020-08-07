using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Services.Implementations
{
    public class SaveFirebasePushNotificationTokenService : ISaveFirebasePushNotificationTokenService
    {
        private readonly ILogger _logger;
        private readonly IPusher _pusher;

        public SaveFirebasePushNotificationTokenService(ILogger logger, IPusher pusher)
        {
            _logger = logger;
            _pusher = pusher;
        }

        /// <summary>
        /// Save the firebase push notification token.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerModel</returns>
        public async Task SaveFirebasePushNotificationToken(Guid deviceId, string token)
        {
            var channelNameReceive = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherChannel.SpacearrChannel}";
            var eventNameReceive = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherEvent.SpacearrEvent}";
            var channelNameSend = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel}";
            var eventNameSend = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherEvent.WorkerServiceEvent}";


            var settings = Task.Run(() => _logger.GetSettingsAsync()).Result;
            if (settings.Count == 0)
            {
                throw new Exception("No settings saved!");
            }

            foreach (var setting in settings)
            {
                try
                {
                    await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                    var firebasePushNotificationDevice = new FirebasePushNotificationDevice { DeviceId = deviceId, Token = token};
                    var pusherSendMessage = new PusherSendMessageModel { Command = CommandType.SaveFirebasePushNotificationTokenCommand, Values = JsonConvert.SerializeObject(firebasePushNotificationDevice) };
                    await _pusher.SendMessage(channelNameSend, eventNameSend, JsonConvert.SerializeObject(pusherSendMessage), setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);
                    await _pusher.WorkerServiceReceiverDisconnect();
                }
                catch (Exception ex)
                {
                    await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                }
            }
        }
    }
}