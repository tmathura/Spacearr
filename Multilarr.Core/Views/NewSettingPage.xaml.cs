using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class NewSettingPage : ContentPage, IDisplayAlertHelper, INavigationPopModalHelper
    {
        public NewSettingPage(ILogger logger)
        {
            InitializeComponent();

            this.BindingContext = new NewSettingDetailViewModel(logger, this, new ValidationHelper(this), this);
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