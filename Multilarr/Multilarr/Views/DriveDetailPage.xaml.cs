using Microcharts;
using Multilarr.ViewModels;
using SkiaSharp;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class DriveDetailPage : ContentPage
    {
        private readonly DriveDetailViewModel _viewModel;

        public DriveDetailPage(DriveDetailViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            var entries = new[]
            {
                new ChartEntry(_viewModel.Drive.TotalSize - _viewModel.Drive.TotalFreeSpace)
                {
                    Label = "Used Space",
                    ValueLabel = $"{_viewModel.Drive.TotalSize - _viewModel.Drive.TotalFreeSpace}",
                    Color = SKColor.Parse("#68B9C0")
                },
                new ChartEntry(_viewModel.Drive.TotalFreeSpace)
                {
                    Label = "Total Free Space",
                    ValueLabel = $"{_viewModel.Drive.TotalFreeSpace}",
                    Color = SKColor.Parse("#90D585"),
                }
            };

            var chart = new PieChart() { Entries = entries };

            this.chartView.Chart = chart;
        }
    }
}