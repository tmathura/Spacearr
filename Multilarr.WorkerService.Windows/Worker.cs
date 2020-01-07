using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Multilarr.WorkerService.Windows.Command;
using Multilarr.WorkerService.Windows.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Multilarr.WorkerService.Windows.Models;
using Newtonsoft.Json;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICommand _command;
        private readonly PusherServer.Pusher _pusherSend;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;

        private const string AppId = "927757";
        private const string Key = "1989c6974272ea96b1c4";
        private const string Secret = "27dd35a15799cb4dac36";
        private const string Cluster = "ap2";

        public Worker(ILogger<Worker> logger, ICommand command)
        {
            _logger = logger;
            _command = command;

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
                PusherReceiveMessage pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(data.ToString());
                var pusherMessageObject = JsonConvert.DeserializeObject<PusherMessageObject>(pusherReceiveMessage.Data);
                ExecuteCommand(pusherMessageObject.Command);
            });
        }

        private void ExecuteCommand(Enumeration.CommandType command)
        {
            var commandObjectSerialized = _command.Invoke(command);
            _pusherSend.TriggerAsync("multilarr-worker-service-windows-channel", "worker_service_event", new { message = commandObjectSerialized.SerializeObject });
        }
    }
}