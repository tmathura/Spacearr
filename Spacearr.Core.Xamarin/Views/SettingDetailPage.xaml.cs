using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
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