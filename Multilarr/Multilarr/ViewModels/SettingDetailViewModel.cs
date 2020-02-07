using Multilarr.Common.Models;

namespace Multilarr.ViewModels
{
    public class SettingDetailViewModel : BaseViewModel
    {
        public SettingLog SettingLog { get; set; }
        public SettingDetailViewModel(SettingLog settingLog = null)
        {
            Title = $"{SettingLog?.ComputerName}";
            SettingLog = settingLog;
        }
    }
}
