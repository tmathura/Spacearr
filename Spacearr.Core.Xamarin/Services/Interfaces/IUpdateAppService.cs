using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Services.Interfaces
{
    public interface IUpdateAppService
    {
        Task Update(string versionNumber);
    }
}
