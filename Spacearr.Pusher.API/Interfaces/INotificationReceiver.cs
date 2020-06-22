﻿using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface INotificationReceiver
    {
        /// <summary>
        /// Connect the notification receiver to the Pusher Pub/Sub..
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task Connect(string appId, string key, string secret, string cluster);
    }
}