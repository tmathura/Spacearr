using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
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
        private readonly IDisplayAlertHelper _displayAlertHelper;
        private readonly INavigationPushModalHelper _navigationPushModalHelper;
        private readonly Page _newSettingPage;

        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<SettingModel> Settings { get; set; }

        public SettingsViewModel(ILogger logger, IDisplayAlertHelper displayAlertHelper, INavigationPushModalHelper navigationPushModalHelper, Page newSettingPage)
        {
            _logger = logger;
            _displayAlertHelper = displayAlertHelper;
            _navigationPushModalHelper = navigationPushModalHelper;
            _newSettingPage = newSettingPage;

            Title = "Settings";
            Settings = new ObservableCollection<SettingModel>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddCommand = new Command(async () => await AddAsync());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Settings.Clear();
                var settings = await _logger.GetSettingsAsync();
                foreach (var setting in settings)
                {
                    Settings.Add(setting);
                }
            }
            catch (Exception ex)
            {
                await _displayAlertHelper.CustomDisplayAlert("Error", ex.Message, "OK");
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
                await _navigationPushModalHelper.CustomPushModalAsync(_newSettingPage);
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