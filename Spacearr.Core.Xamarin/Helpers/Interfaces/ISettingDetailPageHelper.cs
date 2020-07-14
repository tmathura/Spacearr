using Spacearr.Core.Xamarin.Views.Controls.Android;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Helpers.Interfaces
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