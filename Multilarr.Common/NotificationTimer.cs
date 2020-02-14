using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command.MessageCommand;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Multilarr.Common
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly ICommand _command;
        private readonly IPusher _pusher;
        private readonly Timer _timer;

        public NotificationTimer(IConfiguration configuration, ICommand command, IPusher pusher)
        {
            _command = command;
            _pusher = pusher;

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
            var commandObjectSerializedList = new List<CommandObjectSerialized>
            {
                _command.Invoke(Enumeration.CommandType.ComputerDrivesLowCommand)
            };

            foreach (var commandObjectSerialized in commandObjectSerializedList)
            {
                _pusher.SendMessage(Enumeration.PusherChannel.MultilarrWorkerServiceWindowsNotificationChannel.ToString(), Enumeration.PusherEvent.WorkerServiceEvent.ToString(), commandObjectSerialized.SerializeObject);
            }
        }
    }
}