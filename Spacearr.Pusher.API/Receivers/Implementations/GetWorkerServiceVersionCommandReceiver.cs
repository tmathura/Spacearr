using Newtonsoft.Json;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Receivers.Implementations
{
    public class GetWorkerServiceVersionCommandReceiver : IGetWorkerServiceVersionCommandReceiver
    {
        private readonly ILogger _logger;
        private readonly IFileService _fileService;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;
        private readonly string _channelNameSend;
        private readonly string _eventNameSend;

        public GetWorkerServiceVersionCommandReceiver(ILogger logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;

            _channelNameReceive = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel }";
            _eventNameReceive = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherEvent.WorkerServiceEvent }";
            _channelNameSend = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherChannel.SpacearrChannel }";
            _eventNameSend = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherEvent.SpacearrEvent }";
        }

        /// <summary>
        /// Connect the get worker service version receiver to the Pusher Pub/Sub.
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
                        if (deserializeObject.Command == CommandType.GetWorkerServiceVersionCommand)
                        {
                            var command = new GetWorkerServiceVersionCommand(_fileService);
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
