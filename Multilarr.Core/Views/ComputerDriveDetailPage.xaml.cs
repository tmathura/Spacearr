using Microcharts;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using SkiaSharp;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDriveDetailPage : ContentPage
    {
        private readonly ComputerDriveDetailViewModel _viewModel;

        public ComputerDriveDetailPage(ComputerDrive computerDrive)
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
                new ChartEntry(_viewModel.ComputerDrive.TotalUsedSpace)
                {
                    Label = "Used Space",
                    ValueLabel = $"{_viewModel.ComputerDrive.TotalUsedSpaceString}",
                    Color = SKColor.Parse("#68B9C0")
                },
                new ChartEntry(_viewModel.ComputerDrive.TotalFreeSpace)
                {
                    Label = "Total Free Space",
                    ValueLabel = $"{_viewModel.ComputerDrive.TotalFreeSpaceString}",
                    Color = SKColor.Parse("#90D585"),
                }
            };

            var chart = new PieChart { Entries = entries };

            this.chartView.Chart = chart;
        }
    }
}