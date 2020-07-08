using Autofac;
using Plugin.FirebasePushNotification;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Views;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using System;
using Xamarin.Essentials;
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
            
            ThemeLoaderHelper.LoadTheme(Preferences.Get("DarkMode", false));

            if (!Preferences.ContainsKey("DeviceId"))
            {
                Preferences.Set("DeviceId", Guid.NewGuid().ToString());
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                CrossFirebasePushNotificationActions(container.Resolve<ISaveFirebasePushNotificationTokenService>());
            }

            MainPage = new MainPage(container.Resolve<IGetComputerService>(), container.Resolve<ILogger>(), container.Resolve<IPusherValidation>());
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }

        public void CrossFirebasePushNotificationActions(ISaveFirebasePushNotificationTokenService saveFirebasePushNotificationTokenService)
        {
            CrossFirebasePushNotification.Current.RegisterForPushNotifications();
            SaveFirebasePushNotificationToken(saveFirebasePushNotificationTokenService, CrossFirebasePushNotification.Current.Token);
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                SaveFirebasePushNotificationToken(saveFirebasePushNotificationTokenService, p.Token);
            };
        }

        public void SaveFirebasePushNotificationToken(ISaveFirebasePushNotificationTokenService saveFirebasePushNotificationTokenService, string token)
        {
            var deviceId = Guid.Parse(Preferences.Get("DeviceId", Guid.NewGuid().ToString()));
            saveFirebasePushNotificationTokenService.SaveFirebasePushNotificationToken(deviceId, token);
        }
    }
}
