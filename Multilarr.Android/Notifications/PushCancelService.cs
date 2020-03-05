using Android.Content;
using Android.Support.V4.App;
using Multilarr.Core.Notifications;
using Multilarr.Droid.Notifications;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(PushCancelService))]
namespace Multilarr.Droid.Notifications
{
    public class PushCancelService : IPushCancel
    {
        public void CancelPush(int id)
        {
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Cancel(id);
            CreateIntent(id);
        }
        private Intent CreateIntent(int id)
        {
            return new Intent(Application.Context, typeof(MainActivity)).SetAction("LocalNotifierIntent" + id);
        }
    }
}