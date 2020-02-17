using Multilarr.Common;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Multilarr.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public class ComputerDriveService : IComputerDriveService
    {
        private readonly IPusher _pusher;
        private List<ComputerDrive> _computerDrives;

        public ComputerDriveService(IPusher pusher)
        {
            _pusher = pusher;
        }
        
        public async Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync()
        {
            var channelNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.MultilarrChannel.ToString() }";
            var eventNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.MultilarrEvent.ToString() }";
            var channelNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.MultilarrWorkerServiceWindowsChannel.ToString() }";
            var eventNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.WorkerServiceEvent.ToString() }";

            await _pusher.ReceiverConnect(channelNameReceive, eventNameReceive);

            var pusherSendMessage = new PusherSendMessage { Command = Enumeration.CommandType.ComputerDrivesCommand};
            await _pusher.SendMessage(channelNameSend, eventNameSend, JsonConvert.SerializeObject(pusherSendMessage));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_pusher.ReturnData == null)
            {
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("GetComputerDrivesAsync took too long!");
                }
            }

            _computerDrives = JsonConvert.DeserializeObject<List<ComputerDrive>>(_pusher.ReturnData);

            var result = await Task.FromResult(_computerDrives);
            _pusher.ReturnData = null;
            _computerDrives = null;

            await _pusher.ReceiverDisconnect();
            return result;
        }
    }
}