using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class NewSettingPage : ContentPage
    {
        private readonly ILogger _logger;
        public SettingLog Setting { get; set; }

        public NewSettingPage(ILogger logger)
        {
            _logger = logger;

            InitializeComponent();

            Setting = new SettingLog();

            BindingContext = this;
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await _logger.LogSettingAsync(Setting);
            await Navigation.PopModalAsync();
        }
    }
}