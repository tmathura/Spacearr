using Multilarr.Models;
using Multilarr.Services;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDrivesPage : ContentPage
    {
        private readonly ComputerDrivesViewModel _viewModel;

        public ComputerDrivesPage(IComputerDriveDataStore computerDriveDataStore)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ComputerDrivesViewModel(computerDriveDataStore);
        }

        private async void OnComputerDriveSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var computerDrive = (ComputerDrive) args.SelectedItem;
            if (computerDrive == null)
            {
                return;
            }

            await Navigation.PushAsync(new ComputerDriveDetailPage(new ComputerDriveDetailViewModel(computerDrive)));

            ComputerDrivesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadComputerDrivesCommand.Execute(null);
        }
    }
}