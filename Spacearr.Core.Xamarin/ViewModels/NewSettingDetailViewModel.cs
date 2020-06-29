using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Pusher.API.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
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
        public ICommand CancelCommand { get; }
        public ICommand TransitionCommand { get; }
        public bool ShowImageButtons { get; set; }
        public SettingModel SettingModel { get; set; } = new SettingModel();

        public NewSettingDetailViewModel(ILogger logger, IPusherValidation pusherValidation, INewSettingPageHelper newSettingPageHelper, IValidationHelper validationHelper)
        {
            _logger = logger;
            _pusherValidation = pusherValidation;
            _newSettingPageHelper = newSettingPageHelper;
            _validationHelper = validationHelper;

            Title = "New Setting";

            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await CancelAsync());
            TransitionCommand = new Command(async () => await TransitionAsync());
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
                        await _newSettingPageHelper.CustomPopModalAsync();
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

        private async Task CancelAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _newSettingPageHelper.CustomPopModalAsync();
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

        private async Task TransitionAsync()
        {
            if (ShowImageButtons)
            {
                ShowImageButtons = false;

                await _newSettingPageHelper.ViewComfyButton.TranslateTo(_newSettingPageHelper.ViewComfyButton.TranslationX, 50);
                await Task.WhenAll(_newSettingPageHelper.CancelButton.TranslateTo(_newSettingPageHelper.CancelButton.TranslationX, 0, 200, Easing.Linear),
                    _newSettingPageHelper.CancelButton.FadeTo(0, 200),
                    _newSettingPageHelper.SaveButton.FadeTo(0, 200),
                    _newSettingPageHelper.ViewComfyButton.TranslateTo(_newSettingPageHelper.ViewComfyButton.TranslationX, 0, 200, Easing.Linear),
                    _newSettingPageHelper.ViewComfyButton.FadeTo(1, 200));

                _newSettingPageHelper.CancelButton.IsVisible = _newSettingPageHelper.SaveButton.IsVisible = ShowImageButtons;
            }
            else
            {
                ShowImageButtons = _newSettingPageHelper.CancelButton.IsVisible = _newSettingPageHelper.SaveButton.IsVisible = true;

                await Task.WhenAll(_newSettingPageHelper.ViewComfyButton.FadeTo(0, 200),
                    _newSettingPageHelper.ViewComfyButton.TranslateTo(_newSettingPageHelper.ViewComfyButton.TranslationX, 100, 200, Easing.Linear),
                    _newSettingPageHelper.SaveButton.FadeTo(1, 200),
                    _newSettingPageHelper.CancelButton.FadeTo(1, 100),
                    _newSettingPageHelper.CancelButton.TranslateTo(_newSettingPageHelper.CancelButton.TranslationX, -60, 200, Easing.Linear));

                if (Device.RuntimePlatform == Device.Android)
                {
                    _newSettingPageHelper.AndroidEntry.Focus();
                }
                else
                {
                    _newSettingPageHelper.UwpEntry.Focus();
                }
            }
        }
    }
}
