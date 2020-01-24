using Microsoft.Extensions.Hosting;
using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.WorkerService.Windows.Command;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilarr.WorkerService.Windows
{
    public class Worker : BackgroundService
    {
        private readonly ICommand _command;
        private readonly ILogger _logger;
        private readonly PusherServer.IPusher _pusherSend;
        private readonly IPusherClientInterface _pusherReceive;

        private PusherClient.Channel _myChannel;

        public Worker(ICommand command, ILogger logger, PusherServer.IPusher pusherSend, INotificationTimer notificationTimer, IPusherClientInterface pusherReceive)
        {
            _command = command;
            _logger = logger;
            _pusherSend = pusherSend;

            _pusherReceive = pusherReceive;
            _pusherReceive.ConnectAsync();

            notificationTimer.Instantiate();
            _ = SubscribeChannel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
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