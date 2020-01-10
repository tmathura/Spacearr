using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPage(NotificationDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}