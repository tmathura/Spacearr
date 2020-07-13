using Plugin.FirebasePushNotification;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Implementations;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Validator.Interfaces;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage, ISettingsPageHelper
    {
        public Frame NoRowsFrame { get; }

        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly SettingsViewModel _viewModel;

        public SettingsPage(ILogger logger, IPusherValidation pusherValidation)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;

            InitializeComponent();

            ListViewFrame.IsVisible = false;
            NoRowsFrame = ListViewFrame;

            DeviceId.Text = Preferences.Get("DeviceId", Guid.NewGuid().ToString());
            DarkModeSwitch.IsToggled = Preferences.Get("DarkMode", false);

            if (Device.RuntimePlatform == Device.Android)
            {
                FirebaseTokenStackLayout.IsVisible = true;
                FirebaseToken.Text = CrossFirebasePushNotification.Current.Token;
            }

            BindingContext = _viewModel = new SettingsViewModel(logger, this);
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

            if (Device.RuntimePlatform == Device.Android)
            {
                FirebaseToken.Text = CrossFirebasePushNotification.Current.Token;
            }

            _viewModel.LoadItemsCommand.Execute(null);
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }

        public async Task CustomPushAsyncToNewSetting()
        {
            await Navigation.PushAsync(new NewSettingPage(_logger, _pusherValidation));
        }

        private void DarkMode_OnToggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("DarkMode", e.Value);

            ThemeLoaderHelper.LoadTheme(e.Value);
        }
    }
}