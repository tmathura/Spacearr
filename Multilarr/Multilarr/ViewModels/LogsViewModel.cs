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
        public ObservableCollection<Log> Logs { get; set; }
        public Command LoadLogsCommand { get; set; }

        public LogsViewModel()
        {
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
                var logs = await Logger.GetLogsAsync();
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