namespace Multilarr.Common.Models
{
    public class PusherSendMessage
    {
        public Enumeration.CommandType Command { get; set; }
        public string Values { get; set; }
    }
}