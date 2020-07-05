using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using Application = Android.App.Application;

namespace Spacearr.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);


            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
            {
                new NotificationUserCategory("message",new List<NotificationUserAction> {
                    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
                    new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

                }),
                new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
                    new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
                })

            }, false);

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    if (p.Data.ContainsKey("title") && p.Data.ContainsKey("body"))
                    {
                        var mainHandler = new Handler(Looper.MainLooper);
                        Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
                        {
                            Toast.MakeText(this, (string)p.Data["body"], ToastLength.Short).Show();
                        });

                        mainHandler.Post(runnableToast);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            };
        }
    }
}
