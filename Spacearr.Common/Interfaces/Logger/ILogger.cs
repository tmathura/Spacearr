using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Interfaces.Logger
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

        Task<List<SettingModel>> GetSettingsAsync();
        Task LogSettingAsync(SettingModel record);
        Task UpdateSettingAsync(SettingModel record);
        Task DeleteLogAsync(SettingModel record);

        #endregion
    }
}