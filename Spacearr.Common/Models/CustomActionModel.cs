using Spacearr.Common.Enums;
using System.Collections.Generic;

namespace Spacearr.Common.Models
{
    public class CustomActionModel
    {
        public Controls CurrentControl { get; set; }
        public Dictionary<Controls, object> Form { get; set; }
        public string InstallationDirectory { get; set; }
        public AppSettingsModel AppSettingsModel { get; set; }
        public UpdaterAppSettingsModel UpdaterAppSettingsModel { get; set; }
    }
}