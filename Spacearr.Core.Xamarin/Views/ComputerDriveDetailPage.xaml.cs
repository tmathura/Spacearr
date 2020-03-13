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
            
            var entries = new[]
            {
                new ChartEntry(_viewModel.ComputerDriveModel.TotalUsedSpace)
                {
                    Label = "Used Space",
                    ValueLabel = $"{_viewModel.ComputerDriveModel.TotalUsedSpaceString}",
                    Color = SKColor.Parse("#68B9C0")
                },
                new ChartEntry(_viewModel.ComputerDriveModel.TotalFreeSpace)
                {
                    Label = "Total Free Space",
                    ValueLabel = $"{_viewModel.ComputerDriveModel.TotalFreeSpaceString}",
                    Color = SKColor.Parse("#90D585"),
                }
            };

            var chart = new PieChart { Entries = entries };

            this.chartView.Chart = chart;
        }
    }
}