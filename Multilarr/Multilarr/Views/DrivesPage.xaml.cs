using Multilarr.Models;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class DrivesPage : ContentPage
    {
        private readonly DrivesViewModel _viewModel;

        public DrivesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new DrivesViewModel();
        }

        private async void OnDriveSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var drive = (Drive) args.SelectedItem;
            if (drive == null)
            {
                return;
            }

            await Navigation.PushAsync(new DriveDetailPage(new DriveDetailViewModel(drive)));

            // Manually deselect drive.
            DrivesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadDrivesCommand.Execute(null);
        }
    }
}