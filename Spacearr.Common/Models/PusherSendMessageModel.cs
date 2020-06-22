using Spacearr.Common.Enums;

namespace Spacearr.Common.Models
{
    public class PusherSendMessageModel
    {
        public CommandType Command { get; set; }
        public string Values { get; set; }
    }
}