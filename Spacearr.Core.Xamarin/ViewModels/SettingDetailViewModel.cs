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
    public class SettingDetailViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;
        private readonly ISettingDetailPageHelper _settingDetailPageHelper;
        private readonly IValidationHelper _validationHelper;

        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand TransitionCommand { get; }
        public bool ShowImageButtons { get; set; }
        public SettingModel SettingModel { get; set; }

        public SettingDetailViewModel(ILogger logger, IPusherValidation pusherValidation, ISettingDetailPageHelper settingDetailPageHelper, IValidationHelper validationHelper, SettingModel settingModel)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;
            _settingDetailPageHelper = settingDetailPageHelper;
            _validationHelper = validationHelper;
            SettingModel = settingModel;

            Title = $"{SettingModel.ComputerName}";

            DeleteCommand = new Command(async () => await DeleteAsync());
            UpdateCommand = new Command(async () => await UpdateAsync());
            TransitionCommand = new Command(async () => await TransitionAsync());
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
                await _settingDetailPageHelper.CustomDisplayAlert("Success", "Setting Deleted!", "OK");
                await _settingDetailPageHelper.CustomPopAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _settingDetailPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
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
                        await _settingDetailPageHelper.CustomDisplayAlert("Success", "Setting saved!", "OK");
                        await _settingDetailPageHelper.CustomPopAsync();
                    }
                    else
                    {
                        await _settingDetailPageHelper.CustomDisplayAlert("Error", "Pusher details invalid!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _settingDetailPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task TransitionAsync()
        {
            if (ShowImageButtons)
            {
                ShowImageButtons = false;

                await _settingDetailPageHelper.ViewComfyButton.TranslateTo(_settingDetailPageHelper.ViewComfyButton.TranslationX, 50);
                await Task.WhenAll(_settingDetailPageHelper.UpdateButton.TranslateTo(_settingDetailPageHelper.UpdateButton.TranslationX, 0, 200, Easing.Linear),
                    _settingDetailPageHelper.UpdateButton.FadeTo(0, 200),
                    _settingDetailPageHelper.DeleteButton.FadeTo(0, 200),
                    _settingDetailPageHelper.ViewComfyButton.TranslateTo(_settingDetailPageHelper.ViewComfyButton.TranslationX, 0, 200, Easing.Linear),
                    _settingDetailPageHelper.ViewComfyButton.FadeTo(1, 200));

                _settingDetailPageHelper.UpdateButton.IsVisible = _settingDetailPageHelper.DeleteButton.IsVisible = ShowImageButtons;
            }
            else
            {
                ShowImageButtons = _settingDetailPageHelper.UpdateButton.IsVisible = _settingDetailPageHelper.DeleteButton.IsVisible = true;

                await Task.WhenAll(_settingDetailPageHelper.ViewComfyButton.FadeTo(0, 200),
                    _settingDetailPageHelper.ViewComfyButton.TranslateTo(_settingDetailPageHelper.ViewComfyButton.TranslationX, 100, 200, Easing.Linear),
                    _settingDetailPageHelper.DeleteButton.FadeTo(1, 200),
                    _settingDetailPageHelper.UpdateButton.FadeTo(1, 100),
                    _settingDetailPageHelper.UpdateButton.TranslateTo(_settingDetailPageHelper.UpdateButton.TranslationX, -60, 200, Easing.Linear));

                if (Device.RuntimePlatform == Device.Android)
                {
                    _settingDetailPageHelper.AndroidEntry.Focus();
                }
                else
                {
                    _settingDetailPageHelper.UwpEntry.Focus();
                }
            }
        }
    }
}
