using System.Threading.Tasks;

namespace Multilarr.Core.Helpers
{
    public interface IDisplayAlertHelper
    {
        Task CustomDisplayAlert(string title, string message, string cancelText);
    }
}