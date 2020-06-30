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
                computerDrive.ThemeTextColor = (Color)Application.Current.Resources["ThemeTextColor"];
                computerDrive.ThemeLightColor = (Color)Application.Current.Resources["ThemeLightColor"];
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