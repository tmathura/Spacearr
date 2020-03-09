using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helper;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class SettingDetailViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;

        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public SettingModel SettingModel { get; set; }

        public SettingDetailViewModel(Page page, ILogger logger, SettingModel settingModel)
        {
            _page = page;
            _logger = logger;
            SettingModel = settingModel;
            Title = $"{SettingModel?.ComputerName}";

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
                if (ValidationHelper.IsFormValid(SettingModel, _page))
                {
                    var pusherValid = await Pusher.API.Pusher.Validate(SettingModel.PusherAppId, SettingModel.PusherKey, SettingModel.PusherSecret, SettingModel.PusherCluster);
                    if (pusherValid)
                    {
                        await _logger.UpdateSettingAsync(SettingModel);
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
