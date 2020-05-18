using Spacearr.Common.Interfaces;
using Spacearr.Pusher.API.Interfaces;
using System.ServiceProcess;

namespace Spacearr.Windows.Service
{
    public partial class Service : ServiceBase
    {
        private readonly IPusher _pusher;
        private readonly INotificationTimer _notificationTimer;

        public Service(IPusher pusher, INotificationTimer notificationTimer)
        {
            _pusher = pusher;
            _notificationTimer = notificationTimer;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _pusher.ComputerDrivesCommandReceiverConnect();
            _notificationTimer.Instantiate();
        }

        protected override void OnStop()
        {
        }
    }
}
