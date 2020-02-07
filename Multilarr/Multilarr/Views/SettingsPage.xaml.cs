using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsPage(ILogger logger)
        {
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

            await Navigation.PushAsync(new SettingDetailPage(new SettingDetailViewModel(settingLog)));

            SettingsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadSettingsCommand.Execute(null);
        }
    }
}