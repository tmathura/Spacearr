using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Pusher.API.Interfaces.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class ComputerDrivesViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;
        private readonly IComputerDriveService _computerDriveService;

        public ObservableCollection<ComputerDriveModel> ComputerDrives { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ComputerDrivesViewModel(Page page, ILogger logger, IComputerDriveService computerDriveService)
        {
            _page = page;
            _logger = logger;
            _computerDriveService = computerDriveService;

            Title = "Computer Drives";
            ComputerDrives = new ObservableCollection<ComputerDriveModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
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
                _page?.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}