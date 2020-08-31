using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class Pusher : IPusher
    {
        private readonly ILogger _logger;
        private readonly IInvoker _invoker;
        private readonly IWorkerServiceReceiver _workerServiceReceiver;
        private readonly IGetComputerDrivesCommandReceiver _getComputerDrivesCommandReceiver;
        private readonly ISaveFirebasePushNotificationTokenCommandReceiver _saveFirebasePushNotificationTokenCommandReceiver;
        private readonly IGetWorkerServiceVersionCommandReceiver _getWorkerServiceVersionCommandReceiver;
        public TimeSpan TimeLimit => _workerServiceReceiver.TimeLimit;
        public bool CommandCompleted => _workerServiceReceiver.CommandCompleted;
        public List<string> ReturnData => _workerServiceReceiver?.ReturnData;

        public Pusher(ILogger logger, IWorkerServiceReceiver workerServiceReceiver)
        {
            _logger = logger;
            _workerServiceReceiver = workerServiceReceiver;
        }

        public Pusher(ILogger logger, IInvoker invoker, IGetComputerDrivesCommandReceiver getComputerDrivesCommandReceiver, ISaveFirebasePushNotificationTokenCommandReceiver saveFirebasePushNotificationTokenCommandReceiver,
            IGetWorkerServiceVersionCommandReceiver getWorkerServiceVersionCommandReceiver)
        {
            _logger = logger;
            _invoker = invoker;
            _getComputerDrivesCommandReceiver = getComputerDrivesCommandReceiver;
            _saveFirebasePushNotificationTokenCommandReceiver = saveFirebasePushNotificationTokenCommandReceiver;
            _getWorkerServiceVersionCommandReceiver = getWorkerServiceVersionCommandReceiver;
        }

        /// <summary>
        /// Connect the get computer drives command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        public async Task GetComputerDrivesCommandReceiverConnect(string appId, string key, string secret, string cluster)
        {
            await _getComputerDrivesCommandReceiver.Connect(ExecuteCommand, appId, key, secret, cluster);
        }

        /// <summary>
        /// Connect the get Worker Service version command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        public async Task GetWorkerServiceVersionCommandReceiverConnect(string appId, string key, string secret, string cluster)
        {
            await _getWorkerServiceVersionCommandReceiver.Connect(ExecuteCommand, appId, key, secret, cluster);
        }

        /// <summary>
        /// Connect the save firebase push notification token command receiver.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        public async Task SaveFirebasePushNotificationTokenCommandReceiverConnect(string appId, string key, string secret, string cluster)
        {
            await _saveFirebasePushNotificationTokenCommandReceiver.Connect(appId, key, secret, cluster);
        }

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
        public async Task WorkerServiceReceiverConnect(string channelName, string eventName, string appId, string key, string secret, string cluster)
        {
            await _workerServiceReceiver.Connect(channelName, eventName, appId, key, secret, cluster);
        }

        /// <summary>
        /// Disconnect the worker service receiver.
        /// </summary>
        /// <returns></returns>
        public async Task WorkerServiceReceiverDisconnect()
        {
            await _workerServiceReceiver.Disconnect();
        }

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
        public async Task SendMessage(string channelName, string eventName, string message, bool isFinalMessage, string appId, string key, string secret, string cluster)
        {
            if (!string.IsNullOrWhiteSpace(appId) && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret) && !string.IsNullOrWhiteSpace(cluster))
            {
                var pusherSend = new PusherServer.Pusher(appId, key, secret, new PusherServer.PusherOptions { Cluster = cluster });

                await pusherSend.TriggerAsync(channelName, eventName, new { Message = message, IsFinalMessage = isFinalMessage });
            }
            else
            {
                throw new Exception("No default setting saved.");
            }
        }

        /// <summary>
        /// Command to send a message to the Pusher Pub/Sub to a specific channel and event.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="channelName">The channel name to connect to</param>
        /// <param name="eventName">The event name to connect to</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        public async void ExecuteCommand(ICommand command, string channelName, string eventName, string appId, string key, string secret, string cluster)
        {
            try
            {
                var jsonList = await _invoker.Invoke(command);

                for (var i = 0; i < jsonList.Count; i++)
                {
                    await SendMessage(channelName, eventName, jsonList[i], i == jsonList.Count-1, appId, key, secret, cluster);
                }
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message, e.StackTrace);
            }
        }
    }
}
