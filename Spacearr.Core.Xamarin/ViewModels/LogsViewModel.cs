using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly ILogsPageHelper _logsPageHelper;

        public ObservableCollection<LogModel> Logs { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LogsViewModel(ILogger logger, ILogsPageHelper logsPageHelper)
        {
            _logger = logger;
            _logsPageHelper = logsPageHelper;

            Title = "Logs";
            Logs = new ObservableCollection<LogModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
                Logs.Clear();
                await Task.Delay(1000);  //Need this when getting data locally otherwise keeps loading icon on Android.
                var logs = await _logger.GetLogsAsync();
                foreach (var log in logs.OrderByDescending(x => x.LogDate))
                {
                    Logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                await _logsPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}