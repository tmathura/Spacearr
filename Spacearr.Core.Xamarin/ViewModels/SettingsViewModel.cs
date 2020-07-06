using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
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
        private readonly Page _page;

        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<SettingModel> Settings { get; set; }

        public SettingsViewModel(ILogger logger, ISettingsPageHelper settingsPageHelper, Page page)
        {
            _logger = logger;
            _settingsPageHelper = settingsPageHelper;
            _page = page;

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
                await Task.Delay(1000);  //Need this when getting data locally otherwise keeps loading icon on Android.
                var settings = await _logger.GetSettingsAsync();
                foreach (var setting in settings)
                {
                    Settings.Add(setting);
                }
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
                await _settingsPageHelper.CustomPushAsync(_page);
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