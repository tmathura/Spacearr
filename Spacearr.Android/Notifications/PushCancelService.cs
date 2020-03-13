using Android.Content;
using Android.Support.V4.App;
using Spacearr.Core.Xamarin.Notifications;
using Spacearr.Droid.Notifications;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(PushCancelService))]
namespace Spacearr.Droid.Notifications
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