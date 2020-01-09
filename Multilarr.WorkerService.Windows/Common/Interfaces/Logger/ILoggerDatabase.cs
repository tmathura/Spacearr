using Multilarr.WorkerService.Windows.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.WorkerService.Windows.Common.Interfaces.Logger
{
    public interface ILoggerDatabase
    {
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> SaveLogAsync(Log item);
        Task<int> DeleteLogAsync(Log item);
    }
}