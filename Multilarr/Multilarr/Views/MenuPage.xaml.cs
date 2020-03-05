﻿using Multilarr.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private static MainPage RootPage => Application.Current.MainPage as MainPage;

        public MenuPage()
        {
            InitializeComponent();

            var menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title = "Home" },
                new HomeMenuItem {Id = MenuItemType.ComputerDrives, Title = "Computer Drives" },
                new HomeMenuItem {Id = MenuItemType.Logs, Title = "Logs" },
                new HomeMenuItem {Id = MenuItemType.Settings, Title = "Settings" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                {
                    return;
                }

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}