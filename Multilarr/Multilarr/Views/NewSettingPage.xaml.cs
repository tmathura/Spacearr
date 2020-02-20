using Multilarr.Common.Interfaces.Logger;
using Multilarr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Views
{
    [DesignTimeVisible(false)]
    public partial class NewSettingPage : ContentPage
    {
        public NewSettingPage(ILogger logger)
        {
            InitializeComponent();

            this.BindingContext = new NewSettingDetailViewModel(this, logger);
        }
    }
}