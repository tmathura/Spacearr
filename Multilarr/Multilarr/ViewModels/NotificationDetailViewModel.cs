using Multilarr.Common.Models;

namespace Multilarr.ViewModels
{
    public class NotificationDetailViewModel : BaseViewModel
    {
        public NotificationEventArgs Notification { get; set; }
        public NotificationDetailViewModel(NotificationEventArgs notification = null)
        {
            Title = $"{notification?.Title}";
            Notification = notification;
        }
    }
}
