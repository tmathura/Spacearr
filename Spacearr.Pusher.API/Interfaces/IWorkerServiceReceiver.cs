using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IWorkerServiceReceiver
    {
        string ReturnData { get; set; }

        /// <summary>
        /// Connect the worker service receiver to the Pusher Pub/Sub to a specific channel and event.
        /// </summary>
        /// <param name="channelNameReceive">The channel name to connect to</param>
        /// <param name="eventNameReceive">The event name to connect to</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task Connect(string channelNameReceive, string eventNameReceive, string appId, string key, string secret, string cluster);

        /// <summary>
        /// Disconnect the worker service.
        /// </summary>
        /// <returns></returns>
        Task Disconnect();
    }
}