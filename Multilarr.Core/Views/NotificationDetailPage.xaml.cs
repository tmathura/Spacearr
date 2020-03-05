using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
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