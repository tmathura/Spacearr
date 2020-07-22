using Microsoft.Extensions.Configuration;
using Spacearr.Common.Timers.Interfaces;
using Spacearr.Pusher.API;
using System.ServiceProcess;

namespace Spacearr.Windows.Service
{
    public partial class Service : ServiceBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPusher _pusher;
        private readonly ILowSpaceTimer _lowSpaceTimer;

        public Service(IConfiguration configuration, IPusher pusher, ILowSpaceTimer lowSpaceTimer)
        {
            _configuration = configuration;
            _pusher = pusher;
            _lowSpaceTimer = lowSpaceTimer;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _pusher.ComputerDrivesCommandReceiverConnect(_configuration.GetSection("PusherAppId").Value, _configuration.GetSection("PusherKey").Value,
                _configuration.GetSection("PusherSecret").Value, _configuration.GetSection("PusherCluster").Value);
            _pusher.SaveFirebasePushNotificationTokenCommandReceiverConnect(_configuration.GetSection("PusherAppId").Value, _configuration.GetSection("PusherKey").Value,
                _configuration.GetSection("PusherSecret").Value, _configuration.GetSection("PusherCluster").Value);
            _lowSpaceTimer.Instantiate();
        }

        protected override void OnStop()
        {
        }
    }
}
