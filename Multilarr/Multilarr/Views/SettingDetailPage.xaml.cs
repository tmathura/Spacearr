using Multilarr.Common.Interfaces.Logger;
using Multilarr.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingDetailPage : ContentPage
    {
        private readonly ILogger _logger;
        private readonly SettingDetailViewModel _settingDetailViewModel;

        public SettingDetailPage(ILogger logger, SettingDetailViewModel settingDetailViewModel)
        {
            _logger = logger;

            InitializeComponent();

            _settingDetailViewModel = settingDetailViewModel;

            BindingContext = _settingDetailViewModel;
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            await _logger.UpdateSettingAsync(_settingDetailViewModel.SettingLog);
            await Navigation.PopToRootAsync();
        }
    }
}