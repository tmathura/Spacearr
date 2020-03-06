using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IWorkerServiceReceiver
    {
        string ReturnData { get; set; }

        Task Connect(string channelNameReceive, string eventNameReceive, string appId, string key, string secret, string cluster);
        Task Disconnect();
    }
}