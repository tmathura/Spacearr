using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class DriveDetailPage : ContentPage
    {
        public DriveDetailPage(DriveDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}