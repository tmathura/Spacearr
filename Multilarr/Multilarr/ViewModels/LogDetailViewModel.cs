using Multilarr.Common.Models;

namespace Multilarr.Core.ViewModels
{
    public class LogDetailViewModel : BaseViewModel
    {
        public Log Log { get; set; }
        public LogDetailViewModel(Log log = null)
        {
            Title = $"{Log?.Id}";
            Log = log;
        }
    }
}
