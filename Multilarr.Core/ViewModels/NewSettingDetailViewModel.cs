using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class NewSettingDetailViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly INavigationPopModalHelper _navigationPopModalHelper;
        private readonly IValidationHelper _validationHelper;
        private readonly IDisplayAlertHelper _displayAlertHelper;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public SettingModel SettingModel { get; set; } = new SettingModel();

        public NewSettingDetailViewModel(ILogger logger, INavigationPopModalHelper navigationPopModalHelper, IValidationHelper validationHelper, IDisplayAlertHelper displayAlertHelper)
        {
            _logger = logger;
            _navigationPopModalHelper = navigationPopModalHelper;
            _validationHelper = validationHelper;
            _displayAlertHelper = displayAlertHelper;

            Title = "New Setting";

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
                if (_validationHelper.IsFormValid(SettingModel))
                {
                    var pusherValid = await Pusher.API.Pusher.Validate(SettingModel.PusherAppId, SettingModel.PusherKey, SettingModel.PusherSecret, SettingModel.PusherCluster);
                    if (pusherValid)
                    {
                        await _logger.LogSettingAsync(SettingModel);
                        await _displayAlertHelper.CustomDisplayAlert("Success", "Setting saved!", "OK");
                        await _navigationPopModalHelper.CustomPopModalAsync();
                    }
                    else
                    {
                        await _displayAlertHelper.CustomDisplayAlert("Error", "Pusher details invalid!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _displayAlertHelper.CustomDisplayAlert("Error", ex.Message, "OK");
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
                await _navigationPopModalHelper.CustomPopModalAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _displayAlertHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
