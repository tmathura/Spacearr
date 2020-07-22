namespace Spacearr.Common.Models
{
    public class AppSettingsModel
    {
        public bool SendLowSpaceNotification { get; set; }
        public int LowSpaceGBValue { get; set; }
        public int LowSpaceNotificationInterval { get; set; }
        public string PusherAppId { get; set; }
        public string PusherKey { get; set; }
        public string PusherSecret { get; set; }
        public string PusherCluster { get; set; }
    }
}
