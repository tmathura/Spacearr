using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Core.Xamarin.Models;
using Spacearr.Pusher.API.Services.Interfaces;
using Spacearr.Pusher.API.Validator.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        private readonly IGetComputerService _getComputerService;
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly Dictionary<int, NavigationPage> _menuPages = new Dictionary<int, NavigationPage>();

        public MainPage(IGetComputerService getComputerService, ILogger logger, IPusherValidation pusherValidation, IDownloadService downloadService, IUpdateService updateService)
        {
            InitializeComponent();
            
            _getComputerService = getComputerService;
            _logger = logger;
            _pusherValidation = pusherValidation;

            Master = new MenuPage(downloadService, updateService);

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
                        _menuPages.Add(id, new NavigationPage(new ComputersPage(_logger, _getComputerService)));
                        break;
                    case (int)MenuItemType.Logs:
                        _menuPages.Add(id, new NavigationPage(new LogsPage(_logger)));
                        break;
                    case (int)MenuItemType.Settings:
                        _menuPages.Add(id, new NavigationPage(new SettingsPage(_logger, _pusherValidation)));
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