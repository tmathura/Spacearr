using Multilarr.Common.Models;
using Multilarr.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Services
{
    public class NotificationService : INotificationService
    {
        private readonly PusherClient.Pusher _pusherReceive;
        private PusherClient.Channel _myChannel;
        public List<NotificationEventArgs> Notifications = new List<NotificationEventArgs>();
        private readonly INotificationManager _notificationManager;

        public NotificationService(PusherClient.Pusher pusherReceive)
        {
            _pusherReceive = pusherReceive;

            _notificationManager = DependencyService.Get<INotificationManager>();

            _ = SubscribeChannel();
        }
        
        private async Task SubscribeChannel()
        {
            _myChannel = await _pusherReceive.SubscribeAsync("multilarr-worker-service-windows-notification-channel");
            _myChannel.Bind("worker_service_event", (dynamic data) =>
            {
                PusherReceiveMessageObject pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessageObject.Data);
                var deserializeObject = JsonConvert.DeserializeObject<NotificationEventArgs>(pusherReceiveMessage.Message);
                if (_notificationManager != null)
                {
                    deserializeObject.Id = _notificationManager.ScheduleNotification(deserializeObject.Title, deserializeObject.Message);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var mainPage = Application.Current.MainPage as MainPage;
                        mainPage?.DisplayAlert(deserializeObject.Title, deserializeObject.Message, "OK");
                    });
                }
                Notifications.Add(deserializeObject);
            });
        }

        public IEnumerable<NotificationEventArgs> GetNotifications()
        {
            return Notifications;
        }
    }
}