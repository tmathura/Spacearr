using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDrivesPage : ContentPage
    {
        public ComputerDrivesPage(ComputerModel computer)
        {
            InitializeComponent();

            foreach (var computerDrive in computer.ComputerDrives)
            {
                computerDrive.LoadPieChart = true;
                computerDrive.TextColorPrimary = (Color)Application.Current.Resources["TextColorPrimary"];
                computerDrive.ColorPrimaryLight = (Color)Application.Current.Resources["ColorPrimaryLight"];
                computerDrive.MicroChartsFreeSpaceColor = (Color)Application.Current.Resources["MicroChartsFreeSpaceColor"];
                computerDrive.MicroChartsUsedSpaceColor = (Color)Application.Current.Resources["MicroChartsUsedSpaceColor"];
            }

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
    }
}