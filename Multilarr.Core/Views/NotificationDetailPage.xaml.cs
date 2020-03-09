using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPage(NotificationEventArgsModel notification)
        {
            InitializeComponent();

            BindingContext = new NotificationDetailViewModel(notification);
        }
    }
}