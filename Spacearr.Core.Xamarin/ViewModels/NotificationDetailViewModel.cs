using Spacearr.Common.Models;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class NotificationDetailViewModel : BaseViewModel
    {
        public NotificationEventArgsModel Notification { get; set; }
        public NotificationDetailViewModel(NotificationEventArgsModel notification = null)
        {
            Title = $"{notification?.Title}";
            Notification = notification;
        }
    }
}
