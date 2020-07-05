using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Commands;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Command;
using Spacearr.Common.Interfaces.Logger;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Spacearr.Pusher.API
{
    public class NotificationTimer : INotificationTimer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;
        private readonly ISendFirebasePushNotification _sendFirebasePushNotification;

        private readonly Timer _timer;

        public NotificationTimer(IConfiguration configuration, IInvoker invoker, ILogger logger, IComputerDrives computerDrives, ISendFirebasePushNotification sendFirebasePushNotification)
        {
            _configuration = configuration;
            _logger = logger;
            _computerDrives = computerDrives;
            _sendFirebasePushNotification = sendFirebasePushNotification;

            _timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(Convert.ToDouble(configuration.GetSection("NotificationTimerMinutesInterval").Value)).TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += ElapsedEventHandler;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Instantiate()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void DeInstantiate()
        {
            _timer.Stop();
        }

        /// <summary>
        /// The ElapsedEventHandler for the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            var command = new ComputerDrivesLowCommand(_configuration, _logger, _computerDrives, _sendFirebasePushNotification);
            command.Execute();
        }
    }
}