using Spacearr.Common.Interfaces.Logger;
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
    public partial class NewSettingPage : ContentPage, INewSettingPageHelper
    {
        public ImageButton CancelButton { get; }
        public ImageButton SaveButton { get; }
        public ImageButton ViewComfyButton { get; }
        public Entry UwpEntry { get; }
        public CustomEntry AndroidEntry { get; }
        private readonly NewSettingDetailViewModel _viewModel;

        public NewSettingPage(ILogger logger, IPusherValidation pusherValidation)
        {
            InitializeComponent();

            CancelButton = CancelImageButton;
            SaveButton = SaveImageButton;
            ViewComfyButton = ViewComfyImageButton;
            UwpEntry = UwpEntryField;
            AndroidEntry = AndroidEntryField;

            this.BindingContext = _viewModel = new NewSettingDetailViewModel(logger, pusherValidation, this, new ValidationHelper(this));
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }

        public async Task CustomPopModalAsync()
        {
            await Navigation.PopModalAsync();
        }

        private void CustomEntryField_OnUnfocused(object sender, FocusEventArgs e)
        {
            _viewModel.TransitionCommand.Execute(null);
        }
    }
}