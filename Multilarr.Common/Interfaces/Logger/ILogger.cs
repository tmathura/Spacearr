using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Logger
{
    public interface ILogger
    {
        #region Logs
        
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> LogInfoAsync(string logMessage);
        Task<int> LogWarnAsync(string logMessage);
        Task<int> LogErrorAsync(string logMessage);
        Task<int> DeleteLogAsync(Log item);

        #endregion

        #region Notifications

        Task<List<NotificationLog>> GetNotificationLogsAsync();
        Task<NotificationLog> GetNotificationLogAsync(int id);
        Task<int> LogNotificationAsync(string logTitle, string logMessage);
        Task<int> DeleteLogAsync(NotificationLog item);

        #endregion
    }
}