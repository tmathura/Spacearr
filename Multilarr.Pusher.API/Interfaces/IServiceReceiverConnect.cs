using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IServiceReceiverConnect
    {
        string ReturnData { get; set; }

        Task Connect(string channelNameReceive, string eventNameReceive);
        Task ReceiverDisconnect();
    }
}