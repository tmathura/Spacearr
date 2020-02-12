using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command;
using Multilarr.Common.Command.MessageCommand;
using Multilarr.Common.Interfaces;
using Timer = System.Timers.Timer;

namespace Multilarr.Common
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly ICommand _command;
        private readonly PusherServer.IPusher _pusherSend;
        private readonly Timer _timer;

        public NotificationTimer(IConfiguration configuration, ICommand command, PusherServer.IPusher pusherSend)
        {
            _command = command;
            _pusherSend = pusherSend;

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
                _pusherSend.TriggerAsync("multilarr-worker-service-windows-notification-channel", "worker_service_event", new { message = commandObjectSerialized.SerializeObject });
            }
        }
    }
}