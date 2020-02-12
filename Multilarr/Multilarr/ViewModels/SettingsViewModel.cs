using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        public ObservableCollection<SettingLog> Settings { get; set; }
        public Command LoadSettingsCommand { get; set; }

        public SettingsViewModel(ILogger logger)
        {
            _logger = logger;

            Title = "Settings";
            Settings = new ObservableCollection<SettingLog>();
            LoadSettingsCommand = new Command(async () => await ExecuteLoadSettingsCommand());
        }

        private async Task ExecuteLoadSettingsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Settings.Clear();
                var settings = await _logger.GetSettingLogsAsync();
                foreach (var setting in settings)
                {
                    Settings.Add(setting);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}