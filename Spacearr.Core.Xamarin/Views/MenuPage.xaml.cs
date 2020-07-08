using Spacearr.Core.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private static MainPage RootPage => Application.Current.MainPage as MainPage;

        public MenuPage()
        {
            InitializeComponent();

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
                SettingsImageButton.Margin = new Thickness(-8, 0, 0, 0);
                SettingsLabel.Margin = new Thickness(20, 0, 0, 0);
                LogsImageButton.Margin = new Thickness(-8, 0, 0, 0);
                LogsLabel.Margin = new Thickness(20, 0, 0, 0);
            }
            else
            {
                SettingsLabel.Margin = new Thickness(50, 0, 0, 0);
                LogsLabel.Margin = new Thickness(50, 0, 0, 0);
            }
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
                await RootPage.NavigateFromMenu((int)MenuItemType.Logs);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}