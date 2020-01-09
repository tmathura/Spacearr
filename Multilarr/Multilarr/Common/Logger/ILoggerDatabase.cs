using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Logger
{
    public interface ILoggerDatabase
    {
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> SaveLogAsync(Log item);
        Task<int> DeleteLogAsync(Log item);
    }
}