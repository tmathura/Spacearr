using Newtonsoft.Json;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Receivers.Implementations
{
    public class GetComputerDrivesCommandReceiver : IGetComputerDrivesCommandReceiver
    {
        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;
        private readonly string _channelNameSend;
        private readonly string _eventNameSend;

        public GetComputerDrivesCommandReceiver(ILogger logger, IComputerDrives computerDrives)
        {
            _logger = logger;
            _computerDrives = computerDrives;

            _channelNameReceive = $"{ CommandType.GetComputerDrivesCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel }";
            _eventNameReceive = $"{ CommandType.GetComputerDrivesCommand }{ PusherEvent.WorkerServiceEvent }";
            _channelNameSend = $"{ CommandType.GetComputerDrivesCommand }{ PusherChannel.SpacearrChannel }";
            _eventNameSend = $"{ CommandType.GetComputerDrivesCommand }{ PusherEvent.SpacearrEvent }";
        }

        /// <summary>
        /// Connect the computer drives command receiver to the Pusher Pub/Sub.
        /// </summary>
        /// <param name="executeCommand">The command to execute</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        public async Task Connect(Action<ICommand, string, string, string, string, string, string> executeCommand, string appId, string key, string secret, string cluster)
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
                        if (deserializeObject.Command == CommandType.GetComputerDrivesCommand)
                        {
                            var command = new GetComputerDrivesCommand(_computerDrives);
                            executeCommand(command, _channelNameSend, _eventNameSend, appId, key, secret, cluster);
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
