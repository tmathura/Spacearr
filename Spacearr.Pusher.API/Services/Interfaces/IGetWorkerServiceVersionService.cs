using Spacearr.Common.Models;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Services.Interfaces
{
    public interface IGetWorkerServiceVersionService
    {
        /// <summary>
        /// Returns the Worker Service version.
        /// </summary>
        /// <returns>Returns a WorkerServiceVersionModel</returns>
        Task<WorkerServiceVersionModel> GetWorkerServiceVersionServiceAsync(SettingModel setting);
    }
}
