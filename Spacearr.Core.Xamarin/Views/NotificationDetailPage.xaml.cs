using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPage(string title, string message)
        {
            InitializeComponent();

            BindingContext = new NotificationDetailViewModel(title, message);
        }
    }
}