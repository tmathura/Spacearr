using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using Multilarr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class SettingDetailViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly INavigationPopHelper _navigationPopHelper;
        private readonly IValidationHelper _validationHelper;
        private readonly IDisplayAlertHelper _displayAlertHelper;

        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public SettingModel SettingModel { get; set; }

        public SettingDetailViewModel(ILogger logger, IPusherValidation pusherValidation, INavigationPopHelper navigationPopHelper, IValidationHelper validationHelper,
            IDisplayAlertHelper displayAlertHelper, SettingModel settingModel)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;
            _navigationPopHelper = navigationPopHelper;
            _validationHelper = validationHelper;
            _displayAlertHelper = displayAlertHelper;
            SettingModel = settingModel;

            Title = $"{SettingModel.ComputerName}";

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
                await _logger.DeleteLogAsync(SettingModel);
                await _displayAlertHelper.CustomDisplayAlert("Success", "Setting Deleted!", "OK");
                await _navigationPopHelper.CustomPopAsync();
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

        private async Task UpdateAsync()
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
                    var pusherValid = await _pusherValidation.Validate(SettingModel.PusherAppId, SettingModel.PusherKey, SettingModel.PusherSecret, SettingModel.PusherCluster);
                    if (pusherValid)
                    {
                        await _logger.UpdateSettingAsync(SettingModel);
                        await _displayAlertHelper.CustomDisplayAlert("Success", "Setting saved!", "OK");
                        await _navigationPopHelper.CustomPopAsync();
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
    }
}
