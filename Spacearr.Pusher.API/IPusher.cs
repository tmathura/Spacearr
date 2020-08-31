using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public interface IPusher
    {
        TimeSpan TimeLimit { get; }
        bool CommandCompleted { get; }
        List<string> ReturnData { get; }

        /// <summary>
        /// Connect the get computer drives command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task GetComputerDrivesCommandReceiverConnect(string appId, string key, string secret, string cluster);

        /// <summary>
        /// Connect the get Worker Service version command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task GetWorkerServiceVersionCommandReceiverConnect(string appId, string key, string secret, string cluster);

        /// <summary>
        /// Connect the save firebase push notification token command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task SaveFirebasePushNotificationTokenCommandReceiverConnect(string appId, string key, string secret, string cluster);

        /// <summary>
        /// Connect the worker service receiver.
        /// </summary>
        /// <param name="channelName">The channel name to connect to</param>
        /// <param name="eventName">The event name to connect to</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task WorkerServiceReceiverConnect(string channelName, string eventName, string appId, string key, string secret, string cluster);

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
        /// <param name="isFinalMessage">The it is the final message to send</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task SendMessage(string channelName, string eventName, string message, bool isFinalMessage, string appId, string key, string secret, string cluster);
    }
}