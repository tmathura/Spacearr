using Multilarr.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Multilarr.Common.Interfaces;

namespace Multilarr.Common
{
    public class Logger : ILogger
	{
        private readonly ILoggerDatabase _loggerDatabase;

		public Logger(ILoggerDatabase loggerDatabase)
		{
            _loggerDatabase = loggerDatabase;
		}

        #region Logs
        
        public Task<List<Log>> GetLogsAsync()
        {
            return _loggerDatabase.GetLogsAsync();
        }

        public Task<Log> GetLogAsync(int id)
        {
            return _loggerDatabase.GetLogAsync(id);
        }

        public Task<int> LogInfoAsync(string logMessage)
        {
            var log = new Log
            {
                LogMessage = logMessage,
                LogType = Enumeration.LogType.Info,
                LogDate = DateTime.Now
            };

            return _loggerDatabase.SaveLogAsync(log);
        }

        public Task<int> LogWarnAsync(string logMessage)
        {
            var log = new Log
            {
                LogMessage = logMessage,
                LogType = Enumeration.LogType.Warn,
                LogDate = DateTime.Now
            };

            return _loggerDatabase.SaveLogAsync(log);
        }

        public Task<int> LogErrorAsync(string logMessage)
        {
            var log = new Log
            {
                LogMessage = logMessage,
                LogType = Enumeration.LogType.Error,
                LogDate = DateTime.Now
            };

            return _loggerDatabase.SaveLogAsync(log);
        }

        public Task<int> DeleteLogAsync(Log item)
        {
            return _loggerDatabase.DeleteLogAsync(item);
        }

        #endregion

        #region Notifications

        public Task<List<NotificationLog>> GetNotificationLogsAsync()
        {
            return _loggerDatabase.GetNotificationLogsAsync();
        }

        public Task<NotificationLog> GetNotificationLogAsync(int id)
        {
            return _loggerDatabase.GetNotificationLogAsync(id);
        }

        public Task<int> LogNotificationAsync(string logTitle, string logMessage)
        {
            var notificationLog = new NotificationLog
            {
                LogTitle = logTitle,
                LogMessage = logMessage,
                LogDate = DateTime.Now
            };

            return _loggerDatabase.SaveLogAsync(notificationLog);
        }

        public Task<int> DeleteLogAsync(NotificationLog item)
        {
            return _loggerDatabase.DeleteLogAsync(item);
        }

        #endregion

        #region Settings

        public Task<List<SettingLog>> GetSettingLogsAsync()
        {
            return _loggerDatabase.GetSettingLogsAsync();
        }

        public Task<SettingLog> GetSettingLogAsync(int id)
        {
            return _loggerDatabase.GetSettingLogAsync(id);
        }

        public Task<int> LogSettingAsync(SettingLog item)
        {

            item.CreatedDate = DateTime.Now;
            item.UpdatedDate = DateTime.Now;

            return _loggerDatabase.SaveLogAsync(item);
        }

        public Task<int> UpdateSettingAsync(SettingLog item)
        {
            item.UpdatedDate = DateTime.Now;
            return _loggerDatabase.UpdateLogAsync(item);
        }

        public Task<int> DeleteLogAsync(SettingLog item)
        {
            return _loggerDatabase.DeleteLogAsync(item);
        }

        #endregion
    }
}

