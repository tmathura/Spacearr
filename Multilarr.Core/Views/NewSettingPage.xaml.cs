using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class NewSettingPage : ContentPage, IDisplayAlertHelper, INavigationPopModalHelper
    {
        public NewSettingPage(ILogger logger, IPusherValidation pusherValidation)
        {
            InitializeComponent();

            this.BindingContext = new NewSettingDetailViewModel(logger, pusherValidation, this, new ValidationHelper(this), this);
        }

        public async Task CustomDisplayAlert(string title, string message, string cancelText)
        {
            await DisplayAlert(title, message, cancelText);
        }

        public async Task CustomPopModalAsync()
        {
            await Navigation.PopModalAsync();
        }
    }
}