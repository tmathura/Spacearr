using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
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