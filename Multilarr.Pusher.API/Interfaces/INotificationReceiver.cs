using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface INotificationReceiver
    {
        Task Connect(string appId, string key, string secret, string cluster);
    }
}