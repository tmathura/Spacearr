using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface IMenuPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}