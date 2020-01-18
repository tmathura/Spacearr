using Multilarr.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.NotificationLogger
{
	public class NotificationLogger
	{
        private readonly NotificationLoggerDatabase _notificationLoggerDatabase;

		public NotificationLogger(NotificationLoggerDatabase notificationLoggerDatabase)
		{
            _notificationLoggerDatabase = notificationLoggerDatabase;
		}

		public Task<List<NotificationLog>> GetLogsAsync()
		{
			return _notificationLoggerDatabase.GetLogsAsync();
		}

		public Task<NotificationLog> GetLogAsync(int id)
		{
			return _notificationLoggerDatabase.GetLogAsync(id);
		}

		public Task<int> LogNotificationAsync(string logTitle, string logMessage)
        {
			var notificationLog = new NotificationLog
            {
				LogTitle = logTitle,
				LogMessage = logMessage,
                LogDate = DateTime.Now
			};

            return _notificationLoggerDatabase.SaveLogAsync(notificationLog);
        }

		public Task<int> DeleteLogAsync(NotificationLog item)
		{
			return _notificationLoggerDatabase.DeleteLogAsync(item);
		}
	}
}

