using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Pusher.API.Validator.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class NewSettingDetailViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly INewSettingPageHelper _newSettingPageHelper;
        private readonly IValidationHelper _validationHelper;

        public ICommand SaveCommand { get; }
        public SettingModel SettingModel { get; set; } = new SettingModel();

        public NewSettingDetailViewModel(ILogger logger, IPusherValidation pusherValidation, INewSettingPageHelper newSettingPageHelper, IValidationHelper validationHelper)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;
            _newSettingPageHelper = newSettingPageHelper;
            _validationHelper = validationHelper;

            Title = "New Setting";

            SaveCommand = new Command(async () => await SaveAsync());
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
                    var pusherValid = await _pusherValidation.Validate(SettingModel.PusherAppId, SettingModel.PusherKey, SettingModel.PusherSecret, SettingModel.PusherCluster);
                    if (pusherValid)
                    {
                        await _logger.LogSettingAsync(SettingModel);
                        await _newSettingPageHelper.CustomDisplayAlert("Success", "Setting saved!", "OK");
                        await _newSettingPageHelper.CustomPopAsync();
                    }
                    else
                    {
                        await _newSettingPageHelper.CustomDisplayAlert("Error", "Pusher details invalid!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _newSettingPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
