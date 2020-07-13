using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Pusher.API.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class ComputerViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IComputersPageHelper _computersPageHelper;
        private readonly IGetComputerService _getComputerService;

        public ObservableCollection<ComputerModel> Computers { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ComputerViewModel(ILogger logger, IComputersPageHelper computersPageHelper, IGetComputerService getComputerService)
        {
            _logger = logger;
            _computersPageHelper = computersPageHelper;
            _getComputerService = getComputerService;

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
                var computerDrives = await _getComputerService.GetComputersAsync();
                foreach (var computerDrive in computerDrives)
                {
                    Computers.Add(computerDrive);
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message, ex.StackTrace);
                await _computersPageHelper.CustomDisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}