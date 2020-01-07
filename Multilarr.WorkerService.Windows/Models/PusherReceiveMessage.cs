namespace Multilarr.WorkerService.Windows.Models
{
    public class PusherReceiveMessage
    {
        public string Event { get; set; }
        public string Data { get; set; }
        public string Channel { get; set; }
    }
}