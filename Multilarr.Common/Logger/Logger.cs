using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Logger
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
    }
}

