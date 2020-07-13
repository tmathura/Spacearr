using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Helpers.Interfaces
{
    public interface ILogsPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}