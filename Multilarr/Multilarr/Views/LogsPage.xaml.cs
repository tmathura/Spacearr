using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class LogsPage : ContentPage
    {
        private readonly LogsViewModel _viewModel;

        public LogsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new LogsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadLogsCommand.Execute(null);
        }
    }
}