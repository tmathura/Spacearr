using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Service
{
    public class ComputerService : IComputerService
    {
        private readonly ILogger _logger;
        private readonly IPusher _pusher;
        private List<ComputerDriveModel> _computerDrives;

        public ComputerService(ILogger logger, IPusher pusher)
        {
            _logger = logger;
            _pusher = pusher;
        }

        /// <summary>
        /// Returns all the computers and their hard disks.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerModel</returns>
        public async Task<IEnumerable<ComputerModel>> GetComputersAsync()
        {
            var result = new List<ComputerModel>();
            var channelNameReceive = $"{ CommandType.ComputerDrivesCommand }{ PusherChannel.SpacearrChannel}";
            var eventNameReceive = $"{ CommandType.ComputerDrivesCommand }{ PusherEvent.SpacearrEvent}";
            var channelNameSend = $"{ CommandType.ComputerDrivesCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel}";
            var eventNameSend = $"{ CommandType.ComputerDrivesCommand }{ PusherEvent.WorkerServiceEvent}";


            var settings = Task.Run(() => _logger.GetSettingsAsync()).Result;

            foreach (var setting in settings)
            {
                await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                var pusherSendMessage = new PusherSendMessageModel { Command = CommandType.ComputerDrivesCommand };
                await _pusher.SendMessage(channelNameSend, eventNameSend, JsonConvert.SerializeObject(pusherSendMessage), setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                while (_pusher.ReturnData == null)
                {
                    if (stopwatch.ElapsedMilliseconds > 10000)
                    {
                        throw new Exception("GetComputersAsync took too long!");
                    }
                }

                _computerDrives = JsonConvert.DeserializeObject<List<ComputerDriveModel>>(_pusher.ReturnData);

                result.Add(new ComputerModel
                {
                    Name = setting.ComputerName, 
                    ComputerDrives = await Task.FromResult(_computerDrives)
                });
                _computerDrives = null;

                await _pusher.WorkerServiceReceiverDisconnect();
            }

            return result;
        }
    }
}