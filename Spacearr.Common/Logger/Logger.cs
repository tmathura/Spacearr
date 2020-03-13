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
        }

        #region Logs

        public async Task<List<LogModel>> GetLogsAsync()
		{
			return await _database.Table<LogModel>().ToListAsync();
		}

        public async Task LogWarnAsync(string logMessage)
        {
            var record = new LogModel
            {
                LogMessage = logMessage,
                LogType = Enumeration.LogType.Warn,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        public async Task LogErrorAsync(string logMessage, string stackTrace)
        {
            var record = new LogModel
            {
                LogMessage = logMessage,
                LogStackTrace = stackTrace,
                LogType = Enumeration.LogType.Error,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        #endregion

        #region Notifications

        public async Task<List<NotificationModel>> GetNotificationLogsAsync()
        {
            return await _database.Table<NotificationModel>().ToListAsync();
        }

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

        public async Task DeleteLogAsync(NotificationModel record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion

        #region Settings

        public async Task<List<SettingModel>> GetSettingsAsync()
        {
            return await _database.Table<SettingModel>().ToListAsync();
        }

        public async Task LogSettingAsync(SettingModel record)
        {

            record.CreatedDate = DateTime.Now;
            record.UpdatedDate = DateTime.Now;

            await _database.InsertAsync(record);
        }

        public async Task UpdateSettingAsync(SettingModel record)
        {
            record.UpdatedDate = DateTime.Now;
            await _database.UpdateAsync(record);
        }

        public async Task DeleteLogAsync(SettingModel record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion
    }
}

