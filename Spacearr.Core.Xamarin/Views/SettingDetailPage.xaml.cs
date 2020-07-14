using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Implementations;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Core.Xamarin.Views.Controls.Android;
using Spacearr.Pusher.API.Validator.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
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

        private void CustomEntryField_OnUnfocused(object sender, FocusEventArgs e)
        {
            _viewModel.TransitionCommand.Execute(null);
        }
    }
}