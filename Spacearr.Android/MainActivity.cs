﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Work;
using Java.Util.Concurrent;
using Spacearr.Core.Xamarin.Notifications;
using Spacearr.Droid.Notifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Spacearr.Droid
{
    [Activity(Label = "Spacearr", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new Core.Xamarin.App());

            CreateNotificationFromIntent(Intent);

            const string taskId = "NotificationWorker_TaskId";
            var builder = new PeriodicWorkRequest.Builder(typeof(NotificationWorker), 15, TimeUnit.Minutes);
            builder.SetConstraints(Constraints.None);
            var workRequest = builder.Build();
            WorkManager.Instance.EnqueueUniquePeriodicWork(taskId, ExistingPeriodicWorkPolicy.Keep, workRequest);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var statusBarColor = (Color)Xamarin.Forms.Application.Current.Resources["StatusBarColor"];

                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(statusBarColor.ToAndroid());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                var id = intent.Extras.GetInt(AndroidNotificationManager.IdKey);
                var title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                var message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);

                DependencyService.Get<INotificationManager>().ReceiveNotification(id, title, message);
            }
        }
    }
}