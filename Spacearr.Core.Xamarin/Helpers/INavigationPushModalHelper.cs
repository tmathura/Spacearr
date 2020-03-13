using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Helpers
{
    public interface INavigationPushModalHelper
    {
        Task CustomPushModalAsync(Page page);
    }
}