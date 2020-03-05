using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Helper;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class SettingDetailViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;

        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public SettingLog SettingLog { get; set; }

        public SettingDetailViewModel(Page page, ILogger logger, SettingLog settingLog)
        {
            _page = page;
            _logger = logger;
            SettingLog = settingLog;
            Title = $"{SettingLog?.ComputerName}";

            DeleteCommand = new Command(async () => await DeleteAsync());
            UpdateCommand = new Command(async () => await UpdateAsync());
        }
        
        private async Task DeleteAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _logger.DeleteLogAsync(SettingLog);
                await _page.DisplayAlert("Success", "Setting Deleted!", "OK");
                await _page.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                _page?.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (ValidationHelper.IsFormValid(SettingLog, _page))
                {
                    var pusherValid = await Pusher.API.Pusher.Validate(SettingLog.PusherAppId, SettingLog.PusherKey, SettingLog.PusherSecret, SettingLog.PusherCluster);
                    if (pusherValid)
                    {
                        await _logger.UpdateSettingAsync(SettingLog);
                        await _page.DisplayAlert("Success", "Setting saved!", "OK");
                        await _page.Navigation.PopAsync();
                    }
                    else
                    {
                        await _page.DisplayAlert("Error", "Pusher details invalid!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                _page?.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
