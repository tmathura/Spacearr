﻿namespace Spacearr.Pusher.API.Models
{
    public class PusherReceiveMessageObjectModel
    {
        public string Event { get; set; }
        public string Data { get; set; }
        public string Channel { get; set; }
    }
}