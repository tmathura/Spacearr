using Multilarr.Common.Models;

namespace Multilarr.Core.ViewModels
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
