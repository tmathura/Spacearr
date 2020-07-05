using Microsoft.Extensions.Configuration;
using Spacearr.Common.Interfaces;
using Spacearr.Pusher.API.Interfaces;
using System.ServiceProcess;

namespace Spacearr.Windows.Service
{
    public partial class Service : ServiceBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPusher _pusher;
        private readonly INotificationTimer _notificationTimer;

        public Service(IConfiguration configuration, IPusher pusher, INotificationTimer notificationTimer)
        {
            _configuration = configuration;
            _pusher = pusher;
            _notificationTimer = notificationTimer;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _pusher.ComputerDrivesCommandReceiverConnect(_configuration.GetSection("PusherAppId").Value, _configuration.GetSection("PusherKey").Value,
                _configuration.GetSection("PusherSecret").Value, _configuration.GetSection("PusherCluster").Value);
            _pusher.SaveFirebasePushNotificationTokenCommandReceiverConnect(_configuration.GetSection("PusherAppId").Value, _configuration.GetSection("PusherKey").Value,
                _configuration.GetSection("PusherSecret").Value, _configuration.GetSection("PusherCluster").Value);
            _notificationTimer.Instantiate();
        }

        protected override void OnStop()
        {
        }
    }
}
