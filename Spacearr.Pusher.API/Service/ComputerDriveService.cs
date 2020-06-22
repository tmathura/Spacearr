using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
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

        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerDriveModel</returns>
        public async Task<IEnumerable<ComputerDriveModel>> GetComputerDrivesAsync()
        {
            var channelNameReceive = $"{ CommandType.ComputerDrivesCommand }{ PusherChannel.SpacearrChannel.ToString() }";
            var eventNameReceive = $"{ CommandType.ComputerDrivesCommand }{ PusherEvent.SpacearrEvent.ToString() }";
            var channelNameSend = $"{ CommandType.ComputerDrivesCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel.ToString() }";
            var eventNameSend = $"{ CommandType.ComputerDrivesCommand }{ PusherEvent.WorkerServiceEvent.ToString() }";

            await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive);

            var pusherSendMessage = new PusherSendMessageModel { Command = CommandType.ComputerDrivesCommand};
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