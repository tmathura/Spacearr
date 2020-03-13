using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage, IDisplayAlertHelper, INavigationPushModalHelper
    {
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly SettingsViewModel _viewModel;

        public SettingsPage(ILogger logger, IPusherValidation pusherValidation)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;

            InitializeComponent();

            BindingContext = _viewModel = new SettingsViewModel(logger, this, this, new NavigationPage(new NewSettingPage(_logger, _pusherValidation)));
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var settingModel = (SettingModel) args.SelectedItem;
            if (settingModel == null)
            {
                return;
            }

            await Navigation.PushAsync(new SettingDetailPage(_logger, _pusherValidation, settingModel));

            SettingsListView.SelectedItem = null;
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

        public async Task CustomPushModalAsync(Page page)
        {
            await Navigation.PushModalAsync(page);
        }
    }
}