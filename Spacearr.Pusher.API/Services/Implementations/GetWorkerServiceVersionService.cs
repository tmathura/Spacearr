using Newtonsoft.Json;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Services.Implementations
{
    public class GetWorkerServiceVersionService : IGetWorkerServiceVersionService
    {
        private readonly ILogger _logger;
        private readonly IPusher _pusher;

        public GetWorkerServiceVersionService(ILogger logger, IPusher pusher)
        {
            _logger = logger;
            _pusher = pusher;
        }

        /// <summary>
        /// Returns the Worker Service version.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerModel</returns>
        public async Task<WorkerServiceVersionModel> GetWorkerServiceVersionServiceAsync(SettingModel setting)
        {
            WorkerServiceVersionModel workerServiceVersion = null;
            var channelNameReceive = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherChannel.SpacearrChannel}";
            var eventNameReceive = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherEvent.SpacearrEvent}";
            var channelNameSend = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherChannel.SpacearrWorkerServiceWindowsChannel}";
            var eventNameSend = $"{ CommandType.GetWorkerServiceVersionCommand }{ PusherEvent.WorkerServiceEvent}";
            
            try
            {
                await _pusher.WorkerServiceReceiverConnect(channelNameReceive, eventNameReceive, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                var pusherSendMessage = new PusherSendMessageModel { Command = CommandType.GetWorkerServiceVersionCommand };
                await _pusher.SendMessage(channelNameSend, eventNameSend, JsonConvert.SerializeObject(pusherSendMessage), true, setting.PusherAppId, setting.PusherKey, setting.PusherSecret, setting.PusherCluster);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                while (!_pusher.CommandCompleted || stopwatch.ElapsedMilliseconds > _pusher.TimeLimit.TotalMilliseconds)
                {
                    if (stopwatch.ElapsedMilliseconds > _pusher.TimeLimit.TotalMilliseconds)
                    {
                        throw new Exception("Get Worker Service version took too long!");
                    }
                }

                if (_pusher.CommandCompleted)
                {
                    if (_pusher.ReturnData.Count == 0)
                    {
                        throw new Exception("Get Worker Service version has no return data!");
                    }

                    workerServiceVersion = JsonConvert.DeserializeObject<WorkerServiceVersionModel>(_pusher.ReturnData.FirstOrDefault() ?? string.Empty);
                }

                
                await _pusher.WorkerServiceReceiverDisconnect();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
            }

            return workerServiceVersion;
        }
    }
}