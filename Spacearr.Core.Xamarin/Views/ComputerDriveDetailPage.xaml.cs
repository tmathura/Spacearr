using Microcharts;
using SkiaSharp;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDriveDetailPage : ContentPage
    {
        private readonly ComputerDriveDetailViewModel _viewModel;

        public ComputerDriveDetailPage(ComputerDriveModel computerDrive)
        {
            InitializeComponent();

            _viewModel = new ComputerDriveDetailViewModel(computerDrive);
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var textColorPrimary = (Color)Application.Current.Resources["TextColorPrimary"];
            var colorPrimaryLight = (Color)Application.Current.Resources["ColorPrimaryLight"];
            var microChartsFreeSpaceColor = (Color)Application.Current.Resources["MicroChartsFreeSpaceColor"];
            var microChartsUsedSpaceColor = (Color)Application.Current.Resources["MicroChartsUsedSpaceColor"];

            var entries = new[]
            {
                new ChartEntry(_viewModel.ComputerDriveModel.TotalUsedSpace)
                {
                    Label = "Used Space",
                    ValueLabel = $"{_viewModel.ComputerDriveModel.TotalUsedSpaceString}",
                    Color = SKColor.Parse(microChartsUsedSpaceColor.ToHex()),
                    TextColor = SKColor.Parse(textColorPrimary.ToHex())
                },
                new ChartEntry(_viewModel.ComputerDriveModel.TotalFreeSpace)
                {
                    Label = "Total Free Space",
                    ValueLabel = $"{_viewModel.ComputerDriveModel.TotalFreeSpaceString}",
                    Color = SKColor.Parse(microChartsFreeSpaceColor.ToHex()),
                    TextColor = SKColor.Parse(textColorPrimary.ToHex())
                }
            };
            _viewModel.ComputerDriveModel.Chart = new DonutChart { Entries = entries, BackgroundColor = SKColor.Parse(colorPrimaryLight.ToHex()) };
        }
    }
}