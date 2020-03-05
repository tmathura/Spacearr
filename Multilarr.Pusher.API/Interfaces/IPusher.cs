using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IPusher
    {
        string ReturnData { get; }

        void CommandReceiverConnect();
        void ServiceReceiverConnect(string channelName, string eventName);
        void ServiceReceiverDisconnect();
        Task SendMessage(string channelName, string eventName, string message);
    }
}