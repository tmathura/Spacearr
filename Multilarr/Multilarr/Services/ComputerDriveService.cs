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
        private readonly ILogger _logger;
        private List<ComputerDrive> _computerDrives;

        public ComputerDriveService(IPusher pusher, ILogger logger)
        {
            _pusher = pusher;
            _logger = logger;
        }
        
        public async Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync()
        {
            await _pusher.ReceiverConnect("multilarr-worker-service-windows-channel", "worker_service_event");

            var pusherSendMessage = new PusherSendMessage { Command = Enumeration.CommandType.ComputerDrivesCommand};
            await _pusher.SendMessage("multilarr-channel", "multilarr_event", JsonConvert.SerializeObject(pusherSendMessage));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_pusher.ReturnData == null)
            {
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    const string errorMessage = "GetComputerDrivesAsync took too long!";
                    await _logger.LogErrorAsync(errorMessage);
                    throw new Exception(errorMessage);
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