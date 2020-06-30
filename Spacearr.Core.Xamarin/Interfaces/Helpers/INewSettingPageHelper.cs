using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface INewSettingPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPopAsync();
    }
}