using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Pusher
{
    public interface IServiceReceiverConnect
    {
        string ReturnData { get; set; }

        Task Connect(string channelNameReceive, string eventNameReceive);
        Task ReceiverDisconnect();
    }
}