using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Spacearr.Common.Services.Implementations
{
    public class NotificationTimerService : INotificationTimerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;
        private readonly ISendFirebasePushNotificationService _sendFirebasePushNotificationService;

        private readonly Timer _timer;

        public NotificationTimerService(IConfiguration configuration, IInvoker invoker, ILogger logger, IComputerDrives computerDrives, ISendFirebasePushNotificationService sendFirebasePushNotificationService)
        {
            _configuration = configuration;
            _logger = logger;
            _computerDrives = computerDrives;
            _sendFirebasePushNotificationService = sendFirebasePushNotificationService;

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
            var command = new ComputerDrivesLowCommand(_configuration, _logger, _computerDrives, _sendFirebasePushNotificationService);
            command.Execute();
        }
    }
}