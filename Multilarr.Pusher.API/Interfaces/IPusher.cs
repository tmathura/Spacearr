using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IPusher
    {
        string ReturnData { get; }

        void ComputerDrivesCommandReceiverConnect();
        void WorkerServiceReceiverConnect(string channelName, string eventName);
        void WorkerServiceReceiverDisconnect();
        Task SendMessage(string channelName, string eventName, string message);
    }
}