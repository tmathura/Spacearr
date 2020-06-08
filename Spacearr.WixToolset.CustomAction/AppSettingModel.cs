using System.Collections.Generic;

namespace Spacearr.WixToolset.CustomAction
{
    public class AppSettingModel
    {
        public Enumeration.Controls CurrentControl { get; set; }
        public Dictionary<Enumeration.Controls, object> Form { get; set; }
        public string InstallationDirectory { get; set; }
        public string LowComputerDriveGBValue { get; set; }
        public string NotificationTimerMinutesInterval { get; set; }
        public string PusherAppId { get; set; }
        public string PusherKey { get; set; }
        public string PusherSecret { get; set; }
        public string PusherCluster { get; set; }
    }
}