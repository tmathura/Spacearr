using Autofac;
using Plugin.FirebasePushNotification;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using Spacearr.Core.Xamarin.Helpers.Implementations;
using Spacearr.Core.Xamarin.Views;
using Spacearr.Pusher.API.Services.Interfaces;
using Spacearr.Pusher.API.Validator.Interfaces;
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

            VersionTracking.Track();

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

            MainPage = new MainPage(container.Resolve<IGetComputerService>(), container.Resolve<ILogger>(),
                container.Resolve<IPusherValidation>(), container.Resolve<IDownloadService>(), container.Resolve<IUpdateService>(),
                container.Resolve<IGetWorkerServiceVersionService>());
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
