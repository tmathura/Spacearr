using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(Multilarr.Droid.AndroidNotificationManager))]
namespace Multilarr.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        private const string ChannelId = "default";
        private const string ChannelName = "Default";
        private const string ChannelDescription = "The default channel for notifications.";
        private const int PendingIntentId = 0;

        public const string IdKey = "id";
        public const string TitleKey = "title";
        public const string MessageKey = "message";

        private bool _channelInitialized;
        private int _messageId = -1;
        private NotificationManager _manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!_channelInitialized)
            {
                CreateNotificationChannel();
            }

            _messageId++;

            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(IdKey, _messageId);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, PendingIntentId, intent, PendingIntentFlags.OneShot);

            var builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamarin_logo))
                .SetSmallIcon(Resource.Drawable.xamarin_logo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            _manager.Notify(_messageId, notification);

            return _messageId;
        }

        public void ReceiveNotification(int id, string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Id = id,
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        private void CreateNotificationChannel()
        {
            _manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                _manager.CreateNotificationChannel(channel);
            }

            _channelInitialized = true;
        }
    }
}