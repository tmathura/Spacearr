﻿using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class LogsPage : ContentPage, ILogsPageHelper
    {
        private readonly LogsViewModel _viewModel;

        public LogsPage(ILogger logger)
        {
            InitializeComponent();

            BindingContext = _viewModel = new LogsViewModel(logger, this);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
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

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }
    }
}