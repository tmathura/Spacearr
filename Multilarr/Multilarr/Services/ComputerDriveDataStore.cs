using Multilarr.Common;
using Multilarr.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public class ComputerDriveDataStore : IComputerDriveDataStore<ComputerDrive>
    {
        private readonly PusherServer.IPusher _pusherSend;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        private List<ComputerDrive> _computerDrives;

        public ComputerDriveDataStore(PusherServer.IPusher pusherSend, PusherClient.Pusher pusherReceive)
        {
            _pusherSend = pusherSend;
            _pusherReceive = pusherReceive;
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
            while (_computerDrives == null) { }

            var result = await Task.FromResult(_computerDrives);
            _computerDrives = null;
            return result;
        }
    }
}