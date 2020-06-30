using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface ISettingsPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPushAsync(Page page);
    }
}