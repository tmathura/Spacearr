﻿using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces
{
    public interface IPusher
    {
        string ReturnData { get; set; }

        void Connect();
        Task SendMessage(string channelName, string eventName, string message);
        Task ReceiverConnect(string channelName, string eventName);
        Task ReceiverDisconnect();
    }
}