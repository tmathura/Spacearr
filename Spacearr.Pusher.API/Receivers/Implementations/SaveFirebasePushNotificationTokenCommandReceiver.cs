using Newtonsoft.Json;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Receivers.Implementations
{
    public class SaveFirebasePushNotificationTokenCommandReceiver : ISaveFirebasePushNotificationTokenCommandReceiver
    {
        private readonly ILogger _logger;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;

        public SaveFirebasePushNotificationTokenCommandReceiver(ILogger logger)
        {
            _logger = logger;

            _channelNameReceive = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel.ToString() }";
            _eventNameReceive = $"{ CommandType.SaveFirebasePushNotificationTokenCommand }{ PusherEvent.WorkerServiceEvent.ToString() }";
        }

        /// <summary>
        /// Connect the save firebase push notification token command receiver to the Pusher Pub/Sub.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
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
                        PusherReceiveMessageObjectModel pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageObjectModel>(data.ToString());
                        var pusherMessage = JsonConvert.DeserializeObject<PusherReceiveMessageModel>(pusherReceiveMessage.Data);
                        var deserializeObject = JsonConvert.DeserializeObject<PusherSendMessageModel>(pusherMessage.Message);
                        if (deserializeObject.Command == CommandType.SaveFirebasePushNotificationTokenCommand)
                        {
                            var firebasePushNotificationDevice = JsonConvert.DeserializeObject<FirebasePushNotificationDevice>(deserializeObject.Values);
                            var command = new SaveFirebasePushNotificationTokenCommand(_logger, firebasePushNotificationDevice);
                            command.Execute();
                        }
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
