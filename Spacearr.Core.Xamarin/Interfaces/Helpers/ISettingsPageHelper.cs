using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface ISettingsPageHelper
    {
        Frame NoRowsFrame { get; }
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPushAsyncToNewSetting();
    }
}