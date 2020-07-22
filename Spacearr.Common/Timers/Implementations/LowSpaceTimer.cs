using Microsoft.Extensions.Configuration;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.ComputerDrive.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Common.Timers.Interfaces;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Spacearr.Common.Timers.Implementations
{
    public class LowSpaceTimer : ILowSpaceTimer
    {
        private readonly IConfiguration _configuration;
        private readonly IInvoker _invoker;
        private readonly ILogger _logger;
        private readonly IComputerDrives _computerDrives;
        private readonly ISendFirebasePushNotificationService _sendFirebasePushNotificationService;

        private readonly Timer _timer;

        public LowSpaceTimer(IConfiguration configuration, IInvoker invoker, ILogger logger, IComputerDrives computerDrives, ISendFirebasePushNotificationService sendFirebasePushNotificationService)
        {
            _configuration = configuration;
            _invoker = invoker;
            _logger = logger;
            _computerDrives = computerDrives;
            _sendFirebasePushNotificationService = sendFirebasePushNotificationService;

            _timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(Convert.ToDouble(_configuration.GetSection("LowSpaceNotificationInterval").Value)).TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += ElapsedEventHandler;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Instantiate()
        {
            if (Convert.ToBoolean(_configuration.GetSection("SendLowSpaceNotification").Value))
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void DeInstantiate()
        {
            if (Convert.ToBoolean(_configuration.GetSection("SendLowSpaceNotification").Value))
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// The ElapsedEventHandler for the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            var command = new LowSpaceCommand(_configuration, _logger, _computerDrives, _sendFirebasePushNotificationService);
            _invoker.Invoke(command);
        }
    }
}