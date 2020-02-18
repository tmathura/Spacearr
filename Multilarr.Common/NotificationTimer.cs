using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command.Commands;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Pusher;
using Multilarr.Common.Interfaces.Util;
using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Multilarr.Common
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly IConfiguration _configuration;
        private readonly IInvoker _invoker;
        private readonly IPusher _pusher;
        private readonly IDataSize _dataSize;
        private readonly IComputerDrives _computerDrives;

        private readonly Timer _timer;

        public NotificationTimer(IConfiguration configuration, IInvoker invoker, IPusher pusher, IDataSize dataSize, IComputerDrives computerDrives)
        {
            _configuration = configuration;
            _invoker = invoker;
            _pusher = pusher;
            _dataSize = dataSize;
            _computerDrives = computerDrives;

            _timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(Convert.ToDouble(configuration.GetSection("NotificationTimerMinutesInterval").Value)).TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += ElapsedEventHandler;
        }

        public void Instantiate()
        {
            _timer.Start();
        }

        public void DeInstantiate()
        {
            _timer.Stop();
        }

        private void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            var command = new ComputerDrivesLowCommand(_configuration, _dataSize, _computerDrives);
            var jsonList = new List<string>
            {
                _invoker.Invoke(command)
            };

            foreach (var json in jsonList)
            {
                _pusher.SendMessage(Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString(), Enumeration.PusherEvent.WorkerServiceEvent.ToString(), json);
            }
        }
    }
}