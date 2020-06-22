using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Interfaces.Logger
{
    public interface ILogger
    {
        #region Logs

        /// <summary>
        /// Get all the logs.
        /// </summary>
        /// <returns>Return a list of LogModels</returns>
        Task<List<LogModel>> GetLogsAsync();

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="logMessage">The log message</param>
        /// <returns>Returns id</returns>
        Task LogWarnAsync(string logMessage);

        /// <summary>
        /// Log a error.
        /// </summary>
        /// <param name="logMessage">The log message</param>
        /// <param name="stackTrace">The stack trace</param>
        /// <returns>Returns a id</returns>
        Task LogErrorAsync(string logMessage, string stackTrace);

        #endregion

        #region Notifications

        /// <summary>
        /// Log a notification.
        /// </summary>
        /// <param name="logTitle">The notification title</param>
        /// <param name="logMessage">The notification message</param>
        /// <returns>Returns a id</returns>
        Task LogNotificationAsync(string logTitle, string logMessage);

        #endregion

        #region Settings

        /// <summary>
        /// Get all the settings.
        /// </summary>
        /// <returns>Return a list of SettingModels</returns>
        Task<List<SettingModel>> GetSettingsAsync();

        /// <summary>
        /// Log a setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        Task LogSettingAsync(SettingModel record);

        /// <summary>
        /// Update a setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        Task UpdateSettingAsync(SettingModel record);

        /// <summary>
        /// Delete a specific setting.
        /// </summary>
        /// <param name="record">The record to delete</param>
        /// <returns>Returns a id</returns>
        Task DeleteLogAsync(SettingModel record);

        #endregion

        #region XamarinSetting

        /// <summary>
        /// Get all the xamarin settings.
        /// </summary>
        /// <returns>Return a list of XamarinSettingModels</returns>

        Task<List<XamarinSettingModel>> GetXamarinSettingAsync();

        /// <summary>
        /// Log a xamarin setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        Task LogXamarinSettingAsync(XamarinSettingModel record);

        /// <summary>
        /// Update a xamarin setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        Task UpdateXamarinSettingAsync(XamarinSettingModel record);

        #endregion
    }
}