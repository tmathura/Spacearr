using Multilarr.Common.Models;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPage(NotificationEventArgs notification)
        {
            InitializeComponent();

            BindingContext = new NotificationDetailViewModel(notification);
        }
    }
}