namespace Multilarr.Common.Models
{
    public class PusherSendMessageModel
    {
        public Enumeration.CommandType Command { get; set; }
        public string Values { get; set; }
    }
}