using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces.Service;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputersPage : ContentPage, IComputersPageHelper
    {
        private readonly ComputerViewModel _viewModel;

        public ComputersPage(ILogger logger, IComputerService computerDriveService)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ComputerViewModel(logger, this, computerDriveService);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ComputerDrivesListView.SelectedItem = null;

            var computerModel = (ComputerModel) args.SelectedItem;
            if (computerModel == null)
            {
                return;
            }

            if (computerModel.Online)
            {
                await Navigation.PushAsync(new ComputerDrivesPage(computerModel));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }
    }
}