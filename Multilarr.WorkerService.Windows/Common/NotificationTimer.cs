using Multilarr.Common;
using Multilarr.WorkerService.Windows.Command;
using Multilarr.WorkerService.Windows.Command.MessageCommand;
using Multilarr.WorkerService.Windows.Common.Interfaces;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Multilarr.WorkerService.Windows.Common
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly ICommand _command;
        private readonly PusherServer.IPusher _pusherSend;
        private readonly Timer _timer;

        public NotificationTimer(double notificationTimerInterval, ICommand command, PusherServer.IPusher pusherSend)
        {
            _command = command;
            _pusherSend = pusherSend;

            _timer = new Timer
            {
                Interval = notificationTimerInterval,
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