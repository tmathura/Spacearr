using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Helper;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class NewSettingDetailViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public SettingLog SettingLog { get; set; } = new SettingLog();

        public NewSettingDetailViewModel(Page page, ILogger logger)
        {
            _logger = logger;
            Title = $"{SettingLog?.ComputerName}";

            _page = page;

            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await CancelAsync());
        }

        private async Task SaveAsync()
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
                    await _logger.LogSettingAsync(SettingLog);
                    await _page.DisplayAlert("Success", "Setting saved!", "OK");
                    await _page.Navigation.PopModalAsync();
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
                await _page.Navigation.PopModalAsync();
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
