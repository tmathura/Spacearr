using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class LogDetailPage : ContentPage
    {
        public LogDetailPage(Log log)
        {
            InitializeComponent();

            BindingContext = new LogDetailViewModel(log);
        }
    }
}