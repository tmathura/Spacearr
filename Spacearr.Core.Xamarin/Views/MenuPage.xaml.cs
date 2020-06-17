using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Spacearr.Common.Models;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private readonly ILogger _logger;
        private static MainPage RootPage => Application.Current.MainPage as MainPage;

        public MenuPage(ILogger logger)
        {
            _logger = logger;

            InitializeComponent();

            var menuItems = new List<HomeMenuItemModel>
            {
                new HomeMenuItemModel {Id = MenuItemType.Home, Title = "Home" },
                new HomeMenuItemModel {Id = MenuItemType.ComputerDrives, Title = "Computer Drives" },
                new HomeMenuItemModel {Id = MenuItemType.Logs, Title = "Logs" },
                new HomeMenuItemModel {Id = MenuItemType.Settings, Title = "Settings" }
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

            var xamarinSettings = Task.Run(() => _logger.GetXamarinSettingAsync()).Result;

            if (xamarinSettings == null || xamarinSettings.Count == 0)
            {
                DarkTheme.IsToggled = false;
                DarkThemeText.Text = "Dark Theme Off";
            }
            else
            {
                var isDarkTheme = xamarinSettings.First().IsDarkTheme;
                DarkTheme.IsToggled = isDarkTheme;
                DarkThemeText.Text = isDarkTheme ? "Dark Theme On" : "Dark Theme Off";
            }
        }

        private async void OnToggled(object sender, ToggledEventArgs e)
        {
            var xamarinSettings = await _logger.GetXamarinSettingAsync();

            if (xamarinSettings == null || xamarinSettings.Count == 0)
            {
                var xamarinSetting = new XamarinSettingModel { IsDarkTheme = e.Value };
                await _logger.LogXamarinSettingAsync(xamarinSetting);
            }
            else
            {
                xamarinSettings.First().IsDarkTheme = e.Value;
                await _logger.UpdateXamarinSettingAsync(xamarinSettings.First());
            }

            ThemeLoaderHelper.LoadTheme(e.Value);

            DarkThemeText.Text = e.Value ? "Dark Theme On" : "Dark Theme Off";
        }
    }
}