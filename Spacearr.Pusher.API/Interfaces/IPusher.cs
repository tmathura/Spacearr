using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IPusher
    {
        string ReturnData { get; }

        /// <summary>
        /// Connect the computer drives command receiver.
        /// </summary>
        /// <returns></returns>
        Task ComputerDrivesCommandReceiverConnect();

        /// <summary>
        /// Connect the worker service receiver.
        /// </summary>
        /// <param name="channelName">The channel name to connect to</param>
        /// <param name="eventName">The event name to connect to</param>
        /// <returns></returns>
        Task WorkerServiceReceiverConnect(string channelName, string eventName);

        /// <summary>
        /// Disconnect the worker service receiver.
        /// </summary>
        /// <returns></returns>
        Task WorkerServiceReceiverDisconnect();

        /// <summary>
        /// Send a message to the Pusher Pub/Sub to a specific channel and event.
        /// </summary>
        /// <param name="channelName">The channel name to connect to</param>
        /// <param name="eventName">The event name to connect to</param>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        Task SendMessage(string channelName, string eventName, string message);
    }
}