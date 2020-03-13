using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Helpers
{
    public interface IDisplayAlertHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}