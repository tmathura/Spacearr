using Multilarr.Common.Models;
using Multilarr.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class ComputerDrivesViewModel : BaseViewModel
    {
        private readonly IComputerDriveService _computerDriveService;
        public ObservableCollection<ComputerDrive> ComputerDrives { get; set; }
        public Command LoadComputerDrivesCommand { get; set; }

        public ComputerDrivesViewModel(IComputerDriveService computerDriveService)
        {
            _computerDriveService = computerDriveService;

            Title = "Computer Drives";
            ComputerDrives = new ObservableCollection<ComputerDrive>();
            LoadComputerDrivesCommand = new Command(async () => await ExecuteLoadComputerDrivesCommand());
        }

        private async Task ExecuteLoadComputerDrivesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ComputerDrives.Clear();
                var computerDrives = await _computerDriveService.GetComputerDrivesAsync();
                foreach (var computerDrive in computerDrives)
                {
                    ComputerDrives.Add(computerDrive);
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