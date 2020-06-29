using System.Threading.Tasks;
using Spacearr.Core.Xamarin.Controls.Android;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface ISettingDetailPageHelper
    {
        ImageButton UpdateButton { get; }
        ImageButton DeleteButton { get; }
        ImageButton ViewComfyButton { get; }
        Entry UwpEntry { get; }
        CustomEntry AndroidEntry { get; }
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPopAsync();
    }
}