using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class LogsPage : ContentPage, IDisplayAlertHelper
    {
        private readonly LogsViewModel _viewModel;

        public LogsPage(ILogger logger)
        {
            InitializeComponent();

            BindingContext = _viewModel = new LogsViewModel(logger, this);
        }

        private async void OnLogSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var log = (LogModel) args.SelectedItem;
            if (log == null)
            {
                return;
            }

            await Navigation.PushAsync(new LogDetailPage(log));

            LogsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }

        public void CustomDisplayAlert(string title, string message, string cancelText)
        {
            DisplayAlert(title, message, cancelText);
        }
    }
}