using Multilarr.WorkerService.Windows.Common;

namespace Multilarr.WorkerService.Windows.Models
{
    public class PusherMessageObject
    {
        public Enumeration.CommandType Command { get; set; }
        public string Values { get; set; }
    }
}