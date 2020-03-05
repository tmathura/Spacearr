using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
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