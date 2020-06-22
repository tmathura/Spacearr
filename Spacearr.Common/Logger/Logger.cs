using Spacearr.Common.Enums;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Logger
{
    public class Logger : ILogger
    {
        private readonly SQLiteAsyncConnection _database;

        public Logger(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<LogModel>().Wait();
            _database.CreateTableAsync<NotificationModel>().Wait();
            _database.CreateTableAsync<SettingModel>().Wait();
            _database.CreateTableAsync<XamarinSettingModel>().Wait();
        }

        #region Logs

        /// <summary>
        /// Get all the logs.
        /// </summary>
        /// <returns>Return a list of LogModels</returns>
        public async Task<List<LogModel>> GetLogsAsync()
		{
			return await _database.Table<LogModel>().ToListAsync();
		}

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="logMessage">The log message</param>
        /// <returns>Returns id</returns>
        public async Task LogWarnAsync(string logMessage)
        {
            var record = new LogModel
            {
                LogMessage = logMessage,
                LogType = LogType.Warn,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        /// <summary>
        /// Log a error.
        /// </summary>
        /// <param name="logMessage">The log message</param>
        /// <param name="stackTrace">The stack trace</param>
        /// <returns>Returns a id</returns>
        public async Task LogErrorAsync(string logMessage, string stackTrace)
        {
            var record = new LogModel
            {
                LogMessage = logMessage,
                LogStackTrace = stackTrace,
                LogType = LogType.Error,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Get all the notifications.
        /// </summary>
        /// <returns>Return a list of NotificationModels</returns>
        public async Task<List<NotificationModel>> GetNotificationLogsAsync()
        {
            return await _database.Table<NotificationModel>().ToListAsync();
        }

        /// <summary>
        /// Log a notification.
        /// </summary>
        /// <param name="logTitle">The notification title</param>
        /// <param name="logMessage">The notification message</param>
        /// <returns>Returns a id</returns>
        public async Task LogNotificationAsync(string logTitle, string logMessage)
        {
            var record = new NotificationModel
            {
                LogTitle = logTitle,
                LogMessage = logMessage,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        /// <summary>
        /// Delete a specific notification.
        /// </summary>
        /// <param name="record">The record to delete</param>
        /// <returns>Returns a id</returns>
        public async Task DeleteLogAsync(NotificationModel record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion

        #region Settings

        /// <summary>
        /// Get all the settings.
        /// </summary>
        /// <returns>Return a list of SettingModels</returns>
        public async Task<List<SettingModel>> GetSettingsAsync()
        {
            return await _database.Table<SettingModel>().ToListAsync();
        }

        /// <summary>
        /// Log a setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        public async Task LogSettingAsync(SettingModel record)
        {

            record.CreatedDate = DateTime.Now;
            record.UpdatedDate = DateTime.Now;

            await _database.InsertAsync(record);
        }

        /// <summary>
        /// Update a setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        public async Task UpdateSettingAsync(SettingModel record)
        {
            record.UpdatedDate = DateTime.Now;
            await _database.UpdateAsync(record);
        }

        /// <summary>
        /// Delete a specific setting.
        /// </summary>
        /// <param name="record">The record to delete</param>
        /// <returns>Returns a id</returns>
        public async Task DeleteLogAsync(SettingModel record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion

        #region XamarinSetting

        /// <summary>
        /// Get all the xamarin settings.
        /// </summary>
        /// <returns>Return a list of XamarinSettingModels</returns>
        public async Task<List<XamarinSettingModel>> GetXamarinSettingAsync()
        {
            return await _database.Table<XamarinSettingModel>().ToListAsync();
        }

        /// <summary>
        /// Log a xamarin setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        public async Task LogXamarinSettingAsync(XamarinSettingModel record)
        {

            record.CreatedDate = DateTime.Now;
            record.UpdatedDate = DateTime.Now;

            await _database.InsertAsync(record);
        }

        /// <summary>
        /// Update a xamarin setting.
        /// </summary>
        /// <param name="record">The setting</param>
        /// <returns>Returns a id</returns>
        public async Task UpdateXamarinSettingAsync(XamarinSettingModel record)
        {
            record.UpdatedDate = DateTime.Now;
            await _database.UpdateAsync(record);
        }

        #endregion
    }
}

