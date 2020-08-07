using Spacearr.Common.Enums;

namespace Spacearr.Pusher.API.Models
{
    public class PusherSendMessageModel
    {
        public CommandType Command { get; set; }
        public string Values { get; set; }
    }
}