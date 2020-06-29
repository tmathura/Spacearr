using System.Threading.Tasks;
using Spacearr.Core.Xamarin.Controls.Android;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface INewSettingPageHelper
    {
        ImageButton CancelButton { get; }
        ImageButton SaveButton { get; }
        ImageButton ViewComfyButton { get; }
        Entry UwpEntry { get; }
        CustomEntry AndroidEntry { get; }
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPopModalAsync();
    }
}