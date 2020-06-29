﻿using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Controls.Android;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingDetailPage : ContentPage, ISettingDetailPageHelper
    {

        public ImageButton UpdateButton { get; }
        public ImageButton DeleteButton { get; }
        public ImageButton ViewComfyButton { get; }
        public Entry UwpEntry { get; }
        public CustomEntry AndroidEntry { get; }

        private readonly SettingDetailViewModel _viewModel;

        public SettingDetailPage(ILogger logger, IPusherValidation pusherValidation, SettingModel settingModel)
        {
            InitializeComponent();

            UpdateButton = UpdateImageButton;
            DeleteButton = DeleteImageButton;
            ViewComfyButton = ViewComfyImageButton;
            UwpEntry = UwpEntryField;
            AndroidEntry = AndroidEntryField;

            BindingContext = _viewModel = new SettingDetailViewModel(logger, pusherValidation, this, new ValidationHelper(this), settingModel);
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }

        public async Task CustomPopAsync()
        {
            await Navigation.PopAsync();
        }

        public async Task CustomPopModalAsync()
        {
            await Navigation.PopModalAsync();
        }

        public async Task CustomPushModalAsync(Page page)
        {
            await Navigation.PushModalAsync(page);
        }

        private void CustomEntryField_OnUnfocused(object sender, FocusEventArgs e)
        {
            _viewModel.TransitionCommand.Execute(null);
        }
    }
}