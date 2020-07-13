using Microcharts;
using SkiaSharp;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Services.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputersPage : ContentPage, IComputersPageHelper
    {
        private readonly ComputerViewModel _viewModel;

        public ComputersPage(ILogger logger, IGetComputerService getComputerService)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                RefreshImageButton.IsEnabled = false;
                RefreshImageButton.IsVisible = false;
            }

            BindingContext = _viewModel = new ComputerViewModel(logger, this, getComputerService);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ComputerDrivesListView.SelectedItem = null;

            var computerModel = (ComputerModel) args.SelectedItem;
            if (computerModel == null)
            {
                return;
            }

            if (computerModel.Online)
            {
                foreach (var computerDrive in computerModel.ComputerDrives)
                {
                    var textColorPrimary = (Color)Application.Current.Resources["TextColorPrimary"];
                    var colorPrimaryLight = (Color)Application.Current.Resources["ColorPrimaryLight"];
                    var microChartsFreeSpaceColor = (Color)Application.Current.Resources["MicroChartsFreeSpaceColor"];
                    var microChartsUsedSpaceColor = (Color)Application.Current.Resources["MicroChartsUsedSpaceColor"];
                    var entries = new[]
                    {
                        new ChartEntry(computerDrive.TotalUsedSpace)
                        {
                            Label = "Used Space",
                            ValueLabel = $"{computerDrive.TotalUsedSpaceString}",
                            Color = SKColor.Parse(microChartsUsedSpaceColor.ToHex()),
                            TextColor = SKColor.Parse(textColorPrimary.ToHex())
                        },
                        new ChartEntry(computerDrive.TotalFreeSpace)
                        {
                            Label = "Total Free Space",
                            ValueLabel = $"{computerDrive.TotalFreeSpaceString}",
                            Color = SKColor.Parse(microChartsFreeSpaceColor.ToHex()),
                            TextColor = SKColor.Parse(textColorPrimary.ToHex())
                        }
                    };
                    computerDrive.PieChart = new PieChart { Entries = entries, BackgroundColor = SKColor.Parse(colorPrimaryLight.ToHex()) };
                }

                await Navigation.PushAsync(new ComputerDrivesPage(computerModel));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }
    }
}