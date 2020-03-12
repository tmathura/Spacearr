using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces.Service;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDrivesPage : ContentPage, IDisplayAlertHelper
    {
        private readonly ComputerDrivesViewModel _viewModel;

        public ComputerDrivesPage(ILogger logger, IComputerDriveService computerDriveService)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ComputerDrivesViewModel(logger, this, computerDriveService);
        }

        private async void OnComputerDriveSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var computerDrive = (ComputerDriveModel) args.SelectedItem;
            if (computerDrive == null)
            {
                return;
            }

            await Navigation.PushAsync(new ComputerDriveDetailPage(computerDrive));

            ComputerDrivesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }

        public void CustomDisplayAlert(string title, string message, string cancelText)
        {
            DisplayAlert(title, message, cancelText);
        }
    }
}