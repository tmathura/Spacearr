using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Pusher.API.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly ISettingsPageHelper _settingsPageHelper;
        private readonly IGetWorkerServiceVersionService _getWorkerServiceVersionService;

        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<SettingModel> Settings { get; set; }

        public SettingsViewModel(ILogger logger, ISettingsPageHelper settingsPageHelper, IGetWorkerServiceVersionService getWorkerServiceVersionService)
        {
            _logger = logger;
            _settingsPageHelper = settingsPageHelper;
            _getWorkerServiceVersionService = getWorkerServiceVersionService;

            Title = "Settings";
            Settings = new ObservableCollection<SettingModel>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddCommand = new Command(async () => await AddAsync());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Settings.Clear();
                await Task.Delay(1000);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
                var settings = await _logger.GetSettingsAsync();
                foreach (var setting in settings)
                {
                    var workerServiceVersion = await _getWorkerServiceVersionService.GetWorkerServiceVersionServiceAsync(setting);

                    if (workerServiceVersion != null)
                    {
                        setting.Version = workerServiceVersion.Version.ToString();
                    }
                    else
                    {
                        setting.Version = "UNAVAILABLE";
                    }

                    Settings.Add(setting);
                }

                _settingsPageHelper.NoRowsFrame.IsVisible = !(Settings.Count > 0);
            }
            catch (Exception ex)
            {
                await _settingsPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _settingsPageHelper.CustomPushAsyncToNewSetting();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _settingsPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}