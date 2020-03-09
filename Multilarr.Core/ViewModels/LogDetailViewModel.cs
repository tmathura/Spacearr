using Multilarr.Common.Models;

namespace Multilarr.Core.ViewModels
{
    public class LogDetailViewModel : BaseViewModel
    {
        public LogModel Log { get; set; }
        public LogDetailViewModel(LogModel log = null)
        {
            Title = $"{Log?.Id}";
            Log = log;
        }
    }
}
