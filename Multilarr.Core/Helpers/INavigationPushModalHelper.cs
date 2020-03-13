using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.Helpers
{
    public interface INavigationPushModalHelper
    {
        Task CustomPushModalAsync(Page page);
    }
}