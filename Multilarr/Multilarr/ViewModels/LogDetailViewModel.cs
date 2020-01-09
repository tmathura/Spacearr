using Multilarr.Models;

namespace Multilarr.ViewModels
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
