using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface INotificationReceiver
    {
        Task Connect();
    }
}