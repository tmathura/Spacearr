using System;
using Multilarr.Common;
using Multilarr.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Multilarr.Common.Logger;

namespace Multilarr.Services
{
    public class ComputerDriveDataStore : IComputerDriveDataStore
    {
        private readonly PusherServer.IPusher _pusherSend;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        private List<ComputerDrive> _computerDrives;
        private ILogger _logger;

        public ComputerDriveDataStore(PusherServer.IPusher pusherSend, PusherClient.Pusher pusherReceive, ILogger logger)
        {
            _pusherSend = pusherSend;
            _pusherReceive = pusherReceive;
            _logger = logger;

            _ = SubscribeChannel();
        }
        
        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-worker-service-windows-channel");
            _myChannel.Bind("worker_service_event", (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                var deserializeObject = JsonConvert.DeserializeObject<List<ComputerDrive>>(pusherReceiveMessage.Message);
                _computerDrives = deserializeObject;
            });
        }

        public async Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync()
        {
            var pusherSendMessage = new PusherSendMessage { Command = Enumeration.CommandType.ComputerDrivesCommand};
            await _pusherSend.TriggerAsync("multilarr-channel", "multilarr_event", new { message = JsonConvert.SerializeObject(pusherSendMessage) });

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_computerDrives == null)
            {
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    const string errorMessage = "GetComputerDrivesAsync took too long!";
                    await _logger.LogErrorAsync(errorMessage);
                    throw new Exception(errorMessage);
                }
            }

            var result = await Task.FromResult(_computerDrives);
            _computerDrives = null;
            return result;
        }
    }
}