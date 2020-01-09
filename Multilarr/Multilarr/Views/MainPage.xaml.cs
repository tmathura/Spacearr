using Multilarr.Models;
using Multilarr.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Multilarr.Common.Interfaces.Logger;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        private readonly IComputerDriveDataStore _computerDriveDataStore;
        private readonly ILogger _logger;
        private readonly Dictionary<int, NavigationPage> _menuPages = new Dictionary<int, NavigationPage>();

        public MainPage(IComputerDriveDataStore computerDriveDataStore, ILogger logger)
        {
            InitializeComponent();

            _computerDriveDataStore = computerDriveDataStore;
            _logger = logger;

            MasterBehavior = MasterBehavior.Popover;

            _menuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!_menuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        _menuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.ComputerDrives:
                        _menuPages.Add(id, new NavigationPage(new ComputerDrivesPage(_computerDriveDataStore)));
                        break;
                    case (int)MenuItemType.Logs:
                        _menuPages.Add(id, new NavigationPage(new LogsPage(_logger)));
                        break;
                }
            }

            var newPage = _menuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(100);
                }

                IsPresented = false;
            }
        }
    }
}