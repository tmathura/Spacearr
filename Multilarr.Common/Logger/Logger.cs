using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Logger
{
    public class Logger : ILogger
    {
        private readonly SQLiteAsyncConnection _database;

        public Logger(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<LogModel>().Wait();
            _database.CreateTableAsync<NotificationLog>().Wait();
            _database.CreateTableAsync<SettingLog>().Wait();
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

        public async Task<List<NotificationLog>> GetNotificationLogsAsync()
        {
            return await _database.Table<NotificationLog>().ToListAsync();
        }

        public async Task LogNotificationAsync(string logTitle, string logMessage)
        {
            var record = new NotificationLog
            {
                LogTitle = logTitle,
                LogMessage = logMessage,
                LogDate = DateTime.Now
            };

            await _database.InsertAsync(record);
        }

        public async Task DeleteLogAsync(NotificationLog record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion

        #region Settings

        public async Task<List<SettingLog>> GetSettingLogsAsync()
        {
            return await _database.Table<SettingLog>().ToListAsync();
        }

        public async Task LogSettingAsync(SettingLog record)
        {

            record.CreatedDate = DateTime.Now;
            record.UpdatedDate = DateTime.Now;

            await _database.InsertAsync(record);
        }

        public async Task UpdateSettingAsync(SettingLog record)
        {
            record.UpdatedDate = DateTime.Now;
            await _database.UpdateAsync(record);
        }

        public async Task DeleteLogAsync(SettingLog record)
        {
            await _database.DeleteAsync(record);
        }

        #endregion
    }
}

