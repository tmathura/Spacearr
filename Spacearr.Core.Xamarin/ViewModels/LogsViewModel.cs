using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IDisplayAlertHelper _displayAlertHelper;

        public ObservableCollection<LogModel> Logs { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LogsViewModel(ILogger logger, IDisplayAlertHelper displayAlertHelper)
        {
            _logger = logger;
            _displayAlertHelper = displayAlertHelper;

            Title = "Logs";
            Logs = new ObservableCollection<LogModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Logs.Clear();
                var logs = await _logger.GetLogsAsync();
                foreach (var log in logs)
                {
                    Logs.Add(log);
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
    }
}