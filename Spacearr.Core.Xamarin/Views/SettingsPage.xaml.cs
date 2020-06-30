using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage, ISettingsPageHelper
    {
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly SettingsViewModel _viewModel;

        public SettingsPage(ILogger logger, IPusherValidation pusherValidation)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;

            InitializeComponent();

            BindingContext = _viewModel = new SettingsViewModel(logger, this, new NewSettingPage(_logger, _pusherValidation));
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var settingModel = (SettingModel)args.SelectedItem;
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

        public async Task CustomPushAsync(Page page)
        {
            await Navigation.PushAsync(page);
        }
    }
}