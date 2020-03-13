using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views
{
    [DesignTimeVisible(false)]
    public partial class LogDetailPage : ContentPage
    {
        public LogDetailPage(LogModel log)
        {
            InitializeComponent();

            BindingContext = new LogDetailViewModel(log);
        }
    }
}