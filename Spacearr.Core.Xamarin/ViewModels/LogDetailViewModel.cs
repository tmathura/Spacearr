using Spacearr.Common.Models;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class LogDetailViewModel : BaseViewModel
    {
        public LogModel Log { get; set; }
        public LogDetailViewModel(LogModel log)
        {
            Title = $"{log.Id}";
            Log = log;
        }
    }
}
