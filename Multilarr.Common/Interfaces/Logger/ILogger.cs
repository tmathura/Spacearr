using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Logger
{
    public interface ILogger
    {
        #region Logs
        
        Task<List<LogModel>> GetLogsAsync();
        Task LogWarnAsync(string logMessage);
        Task LogErrorAsync(string logMessage, string stackTrace);

        #endregion

        #region Notifications

        Task LogNotificationAsync(string logTitle, string logMessage);

        #endregion

        #region Settings

        Task<List<SettingLog>> GetSettingLogsAsync();
        Task LogSettingAsync(SettingLog record);
        Task UpdateSettingAsync(SettingLog record);
        Task DeleteLogAsync(SettingLog record);

        #endregion
    }
}