using Multilarr.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class DrivesViewModel : BaseViewModel
    {
        public ObservableCollection<Drive> Drives { get; set; }
        public Command LoadDrivesCommand { get; set; }

        public DrivesViewModel()
        {
            Title = "Drives";
            Drives = new ObservableCollection<Drive>();
            LoadDrivesCommand = new Command(async () => await ExecuteLoadDrivesCommand());
        }

        private async Task ExecuteLoadDrivesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Drives.Clear();
                var drives = await DriveDataStore.GetDrivesAsync(true);
                foreach (var drive in drives)
                {
                    Drives.Add(drive);
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