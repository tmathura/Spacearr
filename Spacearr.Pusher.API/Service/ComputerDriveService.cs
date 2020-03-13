using Spacearr.Common;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Service
{
    public class ComputerDriveService : IComputerDriveService
    {
        private readonly IPusher _pusher;
        private List<ComputerDriveModel> _computerDrives;

        public ComputerDriveService(IPusher pusher)
        {
            _pusher = pusher;
        }
        
        public async Task<IEnumerable<ComputerDriveModel>> GetComputerDrivesAsync()
        {
            var channelNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.SpacearrChannel.ToString() }";
            var eventNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.SpacearrEvent.ToString() }";
            var channelNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.SpacearrWorkerServiceWindowsChannel.ToString() }";
            var eventNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.WorkerServiceEvent.ToString() }";

            await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive);

            var pusherSendMessage = new PusherSendMessageModel { Command = Enumeration.CommandType.ComputerDrivesCommand};
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

            _computerDrives = JsonConvert.DeserializeObject<List<ComputerDriveModel>>(_pusher.ReturnData);

            var result = await Task.FromResult(_computerDrives);
            _computerDrives = null;

            await _pusher.WorkerServiceReceiverDisconnect();
            return result;
        }
    }
}