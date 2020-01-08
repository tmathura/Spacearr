using Multilarr.WorkerService.Windows.Common;

namespace Multilarr.WorkerService.Windows.Models
{
    public class PusherSendMessage
    {
        public Enumeration.CommandType Command { get; set; }
        public string Values { get; set; }
    }
}