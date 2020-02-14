using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Services.Interfaces;
using Multilarr.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.ViewModels
{
    public class ComputerDrivesViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IComputerDriveService _computerDriveService;

        public ObservableCollection<ComputerDrive> ComputerDrives { get; set; }
        public Command LoadComputerDrivesCommand { get; set; }

        public ComputerDrivesViewModel(ILogger logger, IComputerDriveService computerDriveService)
        {
            _logger = logger;
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
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                var mainPage = Application.Current.MainPage as MainPage;
                mainPage?.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}