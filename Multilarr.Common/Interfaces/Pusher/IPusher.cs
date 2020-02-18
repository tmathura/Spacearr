using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Pusher
{
    public interface IPusher
    {
        string ReturnData { get; set; }

        void CommandReceiverConnect();
        Task SendMessage(string channelName, string eventName, string message);
        Task ReceiverConnect(string channelName, string eventName);
        Task ReceiverDisconnect();
    }
}