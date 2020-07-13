using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Helpers.Interfaces
{
    public interface ISettingsPageHelper
    {
        Frame NoRowsFrame { get; }
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPushAsyncToNewSetting();
    }
}