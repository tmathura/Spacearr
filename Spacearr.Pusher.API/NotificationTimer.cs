using Microsoft.Extensions.Configuration;
using Spacearr.Common;
using Spacearr.Common.Command.Commands;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Spacearr.Pusher.API
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly IConfiguration _configuration;
        private readonly IInvoker _invoker;
        private readonly IPusher _pusher;
        private readonly IComputerDrives _computerDrives;

        private readonly Timer _timer;

        public NotificationTimer(IConfiguration configuration, IInvoker invoker, IPusher pusher, IComputerDrives computerDrives)
        {
            _configuration = configuration;
            _invoker = invoker;
            _pusher = pusher;
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
            var command = new ComputerDrivesLowCommand(_configuration, _computerDrives);
            var jsonList = new List<string>
            {
                _invoker.Invoke(command)
            };

            foreach (var json in jsonList)
            {
                _pusher.SendMessage(Enumeration.PusherChannel.SpacearrWorkerServiceWindowsNotificationChannel.ToString(), Enumeration.PusherEvent.WorkerServiceEvent.ToString(), json);
            }
        }
    }
}