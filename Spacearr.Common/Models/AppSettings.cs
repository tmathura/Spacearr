namespace Spacearr.Common.Models
{
    public class AppSettings
    {
        public bool SendNotifications { get; set; }
        public int LowComputerDriveGBValue { get; set; }
        public int NotificationTimerMinutesInterval { get; set; }
        public string PusherAppId { get; set; }
        public string PusherKey { get; set; }
        public string PusherSecret { get; set; }
        public string PusherCluster { get; set; }
    }
}
