using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces
{
    public interface ILogger
    {
        #region Logs
        
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> LogInfoAsync(string logMessage);
        Task<int> LogWarnAsync(string logMessage);
        Task<int> LogErrorAsync(string logMessage, string stackTrace);
        Task<int> DeleteLogAsync(Log item);

        #endregion

        #region Notifications

        Task<List<NotificationLog>> GetNotificationLogsAsync();
        Task<NotificationLog> GetNotificationLogAsync(int id);
        Task<int> LogNotificationAsync(string logTitle, string logMessage);
        Task<int> DeleteLogAsync(NotificationLog item);

        #endregion

        #region Settings

        Task<List<SettingLog>> GetSettingLogsAsync();
        Task<SettingLog> GetSettingLogAsync(int id);
        Task<int> LogSettingAsync(SettingLog item);
        Task<int> UpdateSettingAsync(SettingLog item);
        Task<int> DeleteLogAsync(SettingLog item);

        #endregion
    }
}