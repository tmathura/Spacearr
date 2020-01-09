using Multilarr.Common.Interfaces.Logger;
using Multilarr.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        public ObservableCollection<Log> Logs { get; set; }
        public Command LoadLogsCommand { get; set; }

        public LogsViewModel(ILogger logger)
        {
            _logger = logger;

            Title = "Logs";
            Logs = new ObservableCollection<Log>();
            LoadLogsCommand = new Command(async () => await ExecuteLoadLogsCommand());
        }

        private async Task ExecuteLoadLogsCommand()
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
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}