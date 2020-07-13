using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Helpers.Interfaces
{
    public interface INewSettingPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
        Task CustomPopAsync();
    }
}