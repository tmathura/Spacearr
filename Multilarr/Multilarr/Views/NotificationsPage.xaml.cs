using Multilarr.Services;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class NotificationsPage : ContentPage
    {
        private readonly NotificationsViewModel _viewModel;

        public NotificationsPage(INotificationService notificationService)
        {
            InitializeComponent();

            BindingContext = _viewModel = new NotificationsViewModel(notificationService);
        }

        private async void OnNotificationSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var notification = (NotificationEventArgs) args.SelectedItem;
            if (notification == null)
            {
                return;
            }

            await Navigation.PushAsync(new NotificationDetailPage(new NotificationDetailViewModel(notification)));

            NotificationsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadNotificationsCommand.Execute(null);
        }
    }
}