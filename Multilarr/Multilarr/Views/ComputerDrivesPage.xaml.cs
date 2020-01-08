﻿using Multilarr.Models;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class ComputerDrivesPage : ContentPage
    {
        private readonly ComputerDrivesViewModel _viewModel;

        public ComputerDrivesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ComputerDrivesViewModel();
        }

        private async void OnComputerDriveSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var computerDrive = (ComputerDrive) args.SelectedItem;
            if (computerDrive == null)
            {
                return;
            }

            await Navigation.PushAsync(new ComputerDriveDetailPage(new ComputerDriveDetailViewModel(computerDrive)));

            ComputerDrivesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadComputerDrivesCommand.Execute(null);
        }
    }
}