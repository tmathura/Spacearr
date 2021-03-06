﻿using Newtonsoft.Json;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Pusher.API.Models;
using Spacearr.Pusher.API.Receivers.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Receivers.Implementations
{
    public class WorkerServiceReceiver : IWorkerServiceReceiver
    {
        private readonly ILogger _logger;
        public TimeSpan TimeLimit { get; set; }
        public bool CommandCompleted { get; set; }
        public List<string> ReturnData { get; set; }
        private PusherClient.Pusher _pusherReceive;

        public WorkerServiceReceiver(ILogger logger)
        {
            _logger = logger;
        }

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
        public async Task Connect(string channelNameReceive, string eventNameReceive, string appId, string key, string secret, string cluster)
        {
            try
            {
                CommandCompleted = false;
                ReturnData = new List<string>();
                _pusherReceive = null;

                if (!string.IsNullOrWhiteSpace(appId) && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret) && !string.IsNullOrWhiteSpace(cluster))
                {
                    _pusherReceive = new PusherClient.Pusher(key, new PusherClient.PusherOptions { Cluster = cluster });

                    TimeLimit = new TimeSpan(0, 0, 10);
                    var myChannel = await _pusherReceive.SubscribeAsync(channelNameReceive);
                    myChannel.Bind(eventNameReceive, (dynamic data) =>
                    {
                        PusherReceiveMessageObjectModel pusherReceiveMessageObject = JsonConvert.DeserializeObject<PusherReceiveMessageObjectModel>(data.ToString());
                        var pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageModel>(pusherReceiveMessageObject.Data);

                        CommandCompleted = pusherReceiveMessage.IsFinalMessage;
                        ReturnData.Add(pusherReceiveMessage.Message);

                        if (!pusherReceiveMessage.IsFinalMessage)
                        {
                            TimeLimit = TimeLimit.Add(new TimeSpan(0, 0, 10));
                        }
                    });

                    await _pusherReceive.ConnectAsync();
                }
                else
                {
                    throw new Exception("No default setting saved.");
                }
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message, e.StackTrace);
            }
        }

        /// <summary>
        /// Disconnect the worker service.
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            await _pusherReceive.DisconnectAsync();
            _pusherReceive = null;
            ReturnData = null;
        }
    }
}
