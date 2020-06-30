using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class NewSettingPage : ContentPage, INewSettingPageHelper
    {

        public NewSettingPage(ILogger logger, IPusherValidation pusherValidation)
        {
            InitializeComponent();

            BindingContext = new NewSettingDetailViewModel(logger, pusherValidation, this, new ValidationHelper(this));
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }

        public async Task CustomPopAsync()
        {
            await Navigation.PopAsync();
        }
    }
}