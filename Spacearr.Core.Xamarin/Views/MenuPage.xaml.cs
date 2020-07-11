using Octokit;
using Spacearr.Core.Xamarin.Models;
using Spacearr.Core.Xamarin.Services.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private readonly DownloadViewModel _viewModel;
        private static MainPage RootPage => Application.Current.MainPage as MainPage;

        public MenuPage(IDownloadService downloadService)
        {
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
                Version.HeightRequest = 16;
                UpdateProgressBar.WidthRequest = 50;
                UpdateLabel.WidthRequest = 25;
            }
            else
            {
                SettingsLabel.Margin = new Thickness(50, 0, 0, 0);
                LogsLabel.Margin = new Thickness(50, 0, 0, 0);
                Version.HeightRequest = 30;
            }

            BindingContext = _viewModel = new DownloadViewModel(downloadService);
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
                var client = new GitHubClient(new ProductHeaderValue("Spacearr"));
                var releases = await client.Repository.Release.GetAll("tmathura", "Spacearr");
                var latest = releases[0];

                if (VersionTracking.CurrentVersion == latest.TagName)
                {
                    await DisplayAlert("Update", "The latest version is already installed", "Yes", "No");
                }
                else
                {
                    var answer = await DisplayAlert("Update", "There is a new version available, do you want to update?", "Yes", "No");
                    if (answer)
                    {
                        string url;
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            url = latest.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("apk"))?.BrowserDownloadUrl;
                        }
                        else
                        {
                            url = latest.Assets.FirstOrDefault(x => x.Name.ToLower().Contains("uwp"))?.BrowserDownloadUrl;
                        }

                        _viewModel.VersionNumber = latest.TagName;
                        _viewModel.StartDownloadCommand.Execute(url);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}