using Multilarr.Common.Logger;
using System.Collections.Generic;
using System.Threading.Tasks;
using Multilarr.Models;

namespace Multilarr.Common.Interfaces.Logger
{
    public interface ILoggerDatabase
    {
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> SaveLogAsync(Log item);
        Task<int> DeleteLogAsync(Log item);
    }
}