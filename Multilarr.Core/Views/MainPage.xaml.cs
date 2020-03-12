﻿using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Pusher.API.Interfaces.Service;
using Multilarr.Core.Models;
using Multilarr.Core.Notifications;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        private readonly IComputerDriveService _computerDriveService;
        private readonly ILogger _logger;
        private readonly Dictionary<int, NavigationPage> _menuPages = new Dictionary<int, NavigationPage>();

        public MainPage(IComputerDriveService computerDriveService, ILogger logger)
        {
            InitializeComponent();

            var notificationManager = DependencyService.Get<INotificationManager>();
            if (notificationManager != null)
            {
                notificationManager.NotificationReceived += (sender, eventArgs) =>
                {
                    var evtData = (NotificationEventArgsModel)eventArgs;
                    ShowNotification(evtData.Id, evtData.Title, evtData.Message);
                };
            }

            _computerDriveService = computerDriveService;
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
                        _menuPages.Add(id, new NavigationPage(new ComputerDrivesPage(_logger, _computerDriveService)));
                        break;
                    case (int)MenuItemType.Logs:
                        _menuPages.Add(id, new NavigationPage(new LogsPage(_logger)));
                        break;
                    case (int)MenuItemType.Settings:
                        _menuPages.Add(id, new NavigationPage(new SettingsPage(_logger)));
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

        private void ShowNotification(int id, string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var notification = new NotificationEventArgsModel { Id = id, Title = title, Message = message} ;
                var newPage = new NavigationPage(new NotificationDetailPage(notification));
                DependencyService.Get<IPushCancel>().CancelPush(id);
                Detail = newPage;
            });
        }
    }
}