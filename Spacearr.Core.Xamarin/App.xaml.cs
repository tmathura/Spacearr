using Autofac;
using Plugin.FirebasePushNotification;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Views;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using System;
using System.Linq;
using System.Threading.Tasks;
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

            if (Device.RuntimePlatform == Device.Android)
            {
                CrossFirebasePushNotificationActions(logger, container.Resolve<ISaveFirebasePushNotificationTokenService>());
            }

            MainPage = new MainPage(container.Resolve<IComputerService>(), logger, container.Resolve<IPusherValidation>());
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }

        public void CrossFirebasePushNotificationActions(ILogger logger, ISaveFirebasePushNotificationTokenService saveFirebasePushNotificationTokenService)
        {
            CrossFirebasePushNotification.Current.RegisterForPushNotifications();
            SaveFirebasePushNotificationToken(logger, saveFirebasePushNotificationTokenService, CrossFirebasePushNotification.Current.Token);
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                SaveFirebasePushNotificationToken(logger, saveFirebasePushNotificationTokenService, p.Token);
            };
        }

        public void SaveFirebasePushNotificationToken(ILogger logger, ISaveFirebasePushNotificationTokenService saveFirebasePushNotificationTokenService, string token)
        {
            var deviceId = Guid.NewGuid();
            var xamarinSetting = Task.Run(() => logger.GetXamarinSettingAsync()).Result;

            if (xamarinSetting == null || xamarinSetting.Count == 0)
            {
                var setting = new XamarinSettingModel { IsDarkTheme = false, DeviceId = deviceId };
                logger.LogXamarinSettingAsync(setting);
            }
            else
            {
                deviceId = (Guid)xamarinSetting.First().DeviceId;
            }

            saveFirebasePushNotificationTokenService.SaveFirebasePushNotificationToken(deviceId, token);
        }
    }
}
