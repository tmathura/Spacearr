﻿using Spacearr.Common.Interfaces.Command;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IComputerDrivesCommandReceiver
    {
        /// <summary>
        /// Connect the computer drives command receiver to the Pusher Pub/Sub..
        /// </summary>
        /// <param name="executeCommand">The command to execute</param>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns></returns>
        Task Connect(Action<ICommand, string, string> executeCommand, string appId, string key, string secret, string cluster);
    }
}