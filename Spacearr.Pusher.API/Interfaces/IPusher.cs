using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IPusher
    {
        string ReturnData { get; }

        Task ComputerDrivesCommandReceiverConnect();
        Task WorkerServiceReceiverConnect(string channelName, string eventName);
        Task WorkerServiceReceiverDisconnect();
        Task SendMessage(string channelName, string eventName, string message);
    }
}