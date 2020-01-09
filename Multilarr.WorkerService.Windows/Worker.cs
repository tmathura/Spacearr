using Microsoft.Extensions.Hosting;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.WorkerService.Windows.Command;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Multilarr.Common;
using Multilarr.Common.Models;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        private readonly ICommand _command;
        private readonly PusherServer.Pusher _pusherSend;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        private readonly ILogger _logger;

        private const string AppId = "927757";
        private const string Key = "1989c6974272ea96b1c4";
        private const string Secret = "27dd35a15799cb4dac36";
        private const string Cluster = "ap2";

        public Worker(ICommand command, ILogger logger)
        {
            _command = command;
            _logger = logger;

            var optionsSend = new PusherServer.PusherOptions { Cluster = Cluster };
            _pusherSend = new PusherServer.Pusher(AppId, Key, Secret, optionsSend);
            
            var optionsReceive = new PusherClient.PusherOptions { Cluster = Cluster };
            _pusherReceive = new PusherClient.Pusher(Key, optionsReceive);
            _pusherReceive.ConnectAsync();
            _ = SubscribeChannel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-channel");
            _myChannel.Bind("multilarr_event", (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessage.Data);
                var deserializeObject = JsonConvert.DeserializeObject<PusherSendMessage>(pusherMessage.Message);
                ExecuteCommand(deserializeObject.Command);
            });
        }

        private async void ExecuteCommand(Enumeration.CommandType command)
        {
            try
            {
                var commandObjectSerialized = _command.Invoke(command);
                await _pusherSend.TriggerAsync("multilarr-worker-service-windows-channel", "worker_service_event", new { message = commandObjectSerialized.SerializeObject });
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message);
            }
        }
    }
}