using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
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

            BindingContext = _viewModel = new SettingsViewModel(this, logger);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var settingLog = (SettingLog) args.SelectedItem;
            if (settingLog == null)
            {
                return;
            }

            await Navigation.PushAsync(new SettingDetailPage(_logger, settingLog));

            SettingsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}