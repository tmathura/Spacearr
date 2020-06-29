using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Interfaces.Helpers
{
    public interface IComputersPageHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}