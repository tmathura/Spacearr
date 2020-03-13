using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingDetailPage : ContentPage, IDisplayAlertHelper, INavigationPopHelper
    {
        public SettingDetailPage(ILogger logger, IPusherValidation pusherValidation, SettingModel settingModel)
        {
            InitializeComponent();
            
            BindingContext = new SettingDetailViewModel(logger, pusherValidation, this, new ValidationHelper(this), this, settingModel);
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