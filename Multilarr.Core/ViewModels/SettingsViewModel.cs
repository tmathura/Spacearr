using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Views;
using Multilarr.Pusher.API.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;
        private readonly IPusherValidation _pusherValidation;

        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<SettingModel> Settings { get; set; }

        public SettingsViewModel(Page page, ILogger logger, IPusherValidation pusherValidation)
        {
            _page = page;
            _logger = logger;
            _pusherValidation = pusherValidation;

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
                _page?.DisplayAlert("Error", ex.Message, "OK");
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
                await _page.Navigation.PushModalAsync(new NavigationPage(new NewSettingPage(_logger, _pusherValidation)));
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