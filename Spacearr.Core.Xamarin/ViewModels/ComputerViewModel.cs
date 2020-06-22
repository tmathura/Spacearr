using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Pusher.API.Interfaces.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class ComputerViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IDisplayAlertHelper _displayAlertHelper;
        private readonly IComputerService _computerDriveService;

        public ObservableCollection<ComputerModel> Computers { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ComputerViewModel(ILogger logger, IDisplayAlertHelper displayAlertHelper, IComputerService computerDriveService)
        {
            _logger = logger;
            _displayAlertHelper = displayAlertHelper;
            _computerDriveService = computerDriveService;

            Title = "Computer Drives";
            Computers = new ObservableCollection<ComputerModel>();
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
                Computers.Clear();
                var computerDrives = await _computerDriveService.GetComputersAsync();
                foreach (var computerDrive in computerDrives)
                {
                    Computers.Add(computerDrive);
                }
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