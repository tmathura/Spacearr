using Spacearr.Common.Enums;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage, IMenuPageHelper
    {
        private static IUpdateService _updateService;
        private readonly DownloadViewModel _viewModel;
        private static MainPage RootPage => Application.Current.MainPage as MainPage;

        public MenuPage(IDownloadService downloadService, IUpdateService updateService)
        {
            _updateService = updateService;

            InitializeComponent();

            VersionNumberLabel.Text = VersionTracking.CurrentVersion;

            var menuItems = new List<HomeMenuItemModel>
            {
                new HomeMenuItemModel {Id = MenuItemType.Home, Title = "Home" },
                new HomeMenuItemModel {Id = MenuItemType.ComputerDrives, Title = "Computer Drives" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                {
                    return;
                }

                var id = (int)((HomeMenuItemModel)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            if (Device.RuntimePlatform == Device.Android)
            {
                SettingsImageButton.Margin = new Thickness(-7, 0, 0, 0);
                SettingsLabel.Margin = new Thickness(20, 0, 0, 0);
                LogsImageButton.Margin = new Thickness(-7, 0, 0, 0);
                LogsLabel.Margin = new Thickness(20, 0, 0, 0);
                UpdateProgressBar.WidthRequest = 50;
                UpdateLabel.WidthRequest = 25;
            }
            else
            {
                SettingsLabel.Margin = new Thickness(50, 0, 0, 0);
                LogsLabel.Margin = new Thickness(50, 0, 0, 0);
            }

            BindingContext = _viewModel = new DownloadViewModel(downloadService);

            _ = CheckForUpdate();
        }

        private async void SettingsButton_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                ListViewMenu.SelectedItem = null;
                await RootPage.NavigateFromMenu((int) MenuItemType.Settings);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LogsButton_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                ListViewMenu.SelectedItem = null;
                await RootPage.NavigateFromMenu((int)MenuItemType.Logs);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void UpdateAppImageButton_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await CheckForUpdate();
            }
            catch
            {
                await DisplayAlert("Error", "Error checking for update", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task CheckForUpdate()
        {
            if (await _updateService.CheckForUpdateAsync(VersionTracking.CurrentVersion))
            {
                var answer = await DisplayAlert("Update", "There is a new version available, do you want to update?", "Yes", "No");
                if (answer)
                {
                    var url = await _updateService.UpdateUrlOfLastUpdateCheck(Device.RuntimePlatform == Device.Android ? UpdateType.Android : UpdateType.Uwp);
                    var updateAppDownloadModel = new UpdateAppDownloadModel { Url = url, VersionNumber = _updateService.LatestTagName, MenuPage = this };
                    _viewModel.StartUpdateAppDownloadCommand.Execute(updateAppDownloadModel);
                }
            }
            else
            {
                await DisplayAlert("Update", "The latest version is already installed", "Ok");
            }
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }
    }
}