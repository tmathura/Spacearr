﻿using Autofac;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Views;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using System.Linq;
using System.Threading.Tasks;
using Spacearr.Core.Xamarin.Notifications;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            var builder = new ContainerBuilder();
            AutofacConfig.Configure(builder);
            var container = builder.Build();

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<INotificationManager>().Initialize();
            }

            var logger = container.Resolve<ILogger>();

            var xamarinSettings = Task.Run(() => logger.GetXamarinSettingAsync()).Result;

            if (xamarinSettings == null || xamarinSettings.Count == 0)
            {
                ThemeLoaderHelper.LoadTheme(false);
            }
            else
            {
                ThemeLoaderHelper.LoadTheme(xamarinSettings.First().IsDarkTheme);
            }

            MainPage = new MainPage(container.Resolve<IComputerService>(), logger, container.Resolve<IPusherValidation>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
