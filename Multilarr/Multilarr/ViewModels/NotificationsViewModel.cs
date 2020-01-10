using Multilarr.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;
        public ObservableCollection<NotificationEventArgs> Notifications { get; set; }
        public Command LoadNotificationsCommand { get; set; }

        public NotificationsViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;

            Title = "Notifications";
            Notifications = new ObservableCollection<NotificationEventArgs>();
            LoadNotificationsCommand = new Command(ExecuteLoadNotificationsCommand);
        }

        private void ExecuteLoadNotificationsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var notifications = _notificationService.GetNotifications();
                foreach (var notification in notifications)
                {
                    if (!Notifications.Contains(notification))
                    {
                        Notifications.Add(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}