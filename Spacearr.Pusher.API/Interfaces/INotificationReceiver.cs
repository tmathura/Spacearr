using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface INotificationReceiver
    {
        Task Connect(string appId, string key, string secret, string cluster);
    }
}