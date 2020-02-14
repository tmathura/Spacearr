using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {
        private readonly ILogger _logger;
        private readonly SettingsViewModel _viewModel;

        public SettingsPage(ILogger logger)
        {
            _logger = logger;

            InitializeComponent();

            BindingContext = _viewModel = new SettingsViewModel(logger);
        }

        private async void OnSettingSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var settingLog = (SettingLog) args.SelectedItem;
            if (settingLog == null)
            {
                return;
            }

            await Navigation.PushAsync(new SettingDetailPage(_logger, new SettingDetailViewModel(settingLog)));

            SettingsListView.SelectedItem = null;
        }

        async void AddSetting_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewSettingPage(_logger)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadSettingsCommand.Execute(null);
        }
    }
}