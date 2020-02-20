using Multilarr.Common.Models;
using Multilarr.Helper;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Multilarr.Common.Interfaces.Logger;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class SettingDetailViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;

        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }
        public SettingLog SettingLog { get; set; }

        public SettingDetailViewModel(Page page, ILogger logger, SettingLog settingLog)
        {
            _logger = logger;
            SettingLog = settingLog;
            Title = $"{SettingLog?.ComputerName}";

            _page = page;

            UpdateCommand = new Command(async () => await UpdateAsync());
            CancelCommand = new Command(async () => await CancelAsync());
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
                    await _logger.UpdateSettingAsync(SettingLog);
                    await _page.DisplayAlert("Success", "Setting saved!", "OK");
                    await _page.Navigation.PopAsync();
                }
                else
                {
                    return;
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

        private async Task CancelAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
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
    }
}
