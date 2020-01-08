using Multilarr.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Multilarr.Common;
using Newtonsoft.Json;

namespace Multilarr.Services
{
    public class MockDriveDataStore : IDriveDataStore<Drive>
    {
        private readonly PusherServer.IPusher _pusherSend;
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        private List<Drive> driveList;

        public MockDriveDataStore(PusherServer.IPusher pusherSend, PusherClient.Pusher pusherReceive)
        {
            _pusherSend = pusherSend;
            _pusherReceive = pusherReceive;
            _ = SubscribeChannel();
        }

        public static List<Drive> CreateDrives(int total)
        {
            var random = new Random();
            var drives = new List<Drive>();
            for (var i = 0; i < total; i++)
            {
                drives.Add(new Drive
                {
                    Name = $"Drive {i}",
                    RootDirectory = "",
                    VolumeLabel = $"Volume Label {i}",
                    DriveFormat = "FAT32",
                    DriveType = DriveType.Fixed,
                    IsReady = true,
                    TotalFreeSpace = i * random.Next(1, 10),
                    TotalSize = i * random.Next(11, 20)
                });
            }

            return drives;
        }

        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-worker-service-windows-channel");
            _myChannel.Bind("worker_service_event", (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                var deserializeObject = JsonConvert.DeserializeObject<List<Drive>>(pusherReceiveMessage.Message);
                driveList = deserializeObject;
            });
        }

        public async Task<Drive> GetDriveAsync(string name)
        {
            return await Task.FromResult(CreateDrives(10).FirstOrDefault(s => s.Name == name));
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(bool forceRefresh = false)
        {
            var pusherSendMessage = new PusherSendMessage { Command = Enumeration.CommandType.DrivesCommand};
            await _pusherSend.TriggerAsync("multilarr-channel", "multilarr_event", new { message = JsonConvert.SerializeObject(pusherSendMessage) });
            while (driveList == null) { }
            return await Task.FromResult(driveList);
        }
    }
}