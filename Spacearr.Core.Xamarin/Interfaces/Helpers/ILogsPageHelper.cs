using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface ILogsPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}