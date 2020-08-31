using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Services.Implementations
{
    public class GetComputerService : IGetComputerService
    {
        private readonly ILogger _logger;
        private readonly IPusher _pusher;

        public GetComputerService(ILogger logger, IPusher pusher)
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
            var channelNameReceive = $"{ CommandType.GetComputerDrivesCommand }{ PusherChannel.SpacearrChannel}";
            var eventNameReceive = $"{ CommandType.GetComputerDrivesCommand }{ PusherEvent.SpacearrEvent}";
            var channelNameSend = $"{ CommandType.GetComputerDrivesCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel}";
            var eventNameSend = $"{ CommandType.GetComputerDrivesCommand }{ PusherEvent.WorkerServiceEvent}";


            var settings = Task.Run(() => _logger.GetSettingsAsync()).Result;
            if (settings.Count == 0)
            {
                throw new Exception("No settings saved!");
            }

            foreach (var setting in settings)
            {
                try
                {
                    await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                    var pusherSendMessage = new PusherSendMessageModel { Command = CommandType.GetComputerDrivesCommand };
                    await _pusher.SendMessage(channelNameSend, eventNameSend, JsonConvert.SerializeObject(pusherSendMessage), true, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (!_pusher.CommandCompleted || stopwatch.ElapsedMilliseconds > _pusher.TimeLimit.TotalMilliseconds)
                    {
                        if (stopwatch.ElapsedMilliseconds > _pusher.TimeLimit.TotalMilliseconds)
                        {
                            throw new Exception("Get computer information took too long!");
                        }
                    }

                    if (_pusher.CommandCompleted)
                    {
                        if (_pusher.ReturnData.Count == 0)
                        {
                            throw new Exception("Get computer information has no return data!");
                        }

                        var computerDrives = new List<ComputerDriveModel>();
                        foreach (var json in _pusher.ReturnData)
                        {
                            computerDrives.Add(JsonConvert.DeserializeObject<ComputerDriveModel>(json));
                        }

                        result.Add(new ComputerModel
                        {
                            Name = setting.ComputerName,
                            ComputerDrives = await Task.FromResult(computerDrives)
                        });
                    }

                    await _pusher.WorkerServiceReceiverDisconnect();
                }
                catch (Exception ex)
                {
                    result.Add(new ComputerModel
                    {
                        Name = setting.ComputerName,
                        ComputerDrives = new List<ComputerDriveModel>(),
                        Error = $"UNAVAILABLE: {ex.Message}"
                    });
                    await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                }
            }

            return result;
        }
    }
}