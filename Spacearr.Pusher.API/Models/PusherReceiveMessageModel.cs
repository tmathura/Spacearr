namespace Spacearr.Pusher.API.Models
{
    public class PusherReceiveMessageModel
    {
        public string Message { get; set; }
        public bool IsFinalMessage { get; set; }
    }
}