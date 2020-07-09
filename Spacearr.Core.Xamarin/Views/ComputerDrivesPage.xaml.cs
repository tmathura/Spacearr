using Microcharts;
using SkiaSharp;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDrivesPage : ContentPage
    {
        private readonly ComputerModel _computer;

        public ComputerDrivesPage(ComputerModel computer)
        {
            InitializeComponent();

            _computer = computer;

            var viewModel = new ComputerDriveViewModel(computer);
            BindingContext = viewModel;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var computerDrive = (ComputerDriveModel)args.SelectedItem;
            if (computerDrive == null)
            {
                return;
            }

            await Navigation.PushAsync(new ComputerDriveDetailPage(computerDrive));

            ComputerDrivesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            foreach (var computerDrive in _computer.ComputerDrives)
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
        }
    }
}