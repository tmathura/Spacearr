﻿using Multilarr.Models;
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

        private async void OnLogSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var log = (Log) args.SelectedItem;
            if (log == null)
            {
                return;
            }

            await Navigation.PushAsync(new LogDetailPage(new LogDetailViewModel(log)));

            LogsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadLogsCommand.Execute(null);
        }
    }
}