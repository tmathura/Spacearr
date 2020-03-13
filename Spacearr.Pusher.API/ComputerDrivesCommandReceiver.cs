using Newtonsoft.Json;
using Spacearr.Common;
using Spacearr.Common.Command.Commands;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class ComputerDrivesCommandReceiver : IComputerDrivesCommandReceiver
    {
        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;
        private readonly string _channelNameSend;
        private readonly string _eventNameSend;

        public ComputerDrivesCommandReceiver(ILogger logger, IComputerDrives computerDrives)
        {
            _logger = logger;
            _computerDrives = computerDrives;

            _channelNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.SpacearrWorkerServiceWindowsChannel.ToString() }";
            _eventNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.WorkerServiceEvent.ToString() }";
            _channelNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.SpacearrChannel.ToString() }";
            _eventNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.SpacearrEvent.ToString() }";
        }

        public async Task Connect(Action<ICommand, string, string> executeCommand, string appId, string key, string secret, string cluster)
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
                        if (deserializeObject.Command == Enumeration.CommandType.ComputerDrivesCommand)
                        {
                            var command = new ComputerDrivesCommand(_computerDrives);
                            executeCommand(command, _channelNameSend, _eventNameSend);
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
