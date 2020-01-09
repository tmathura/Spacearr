using Multilarr.Common.Interfaces.Logger;
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
				LogType = Enumeration.LogType.Info
			};

            return _loggerDatabase.SaveLogAsync(log);
        }

		public Task<int> LogWarnAsync(string logMessage)
        {
			var log = new Log
            {
				LogMessage = logMessage,
				LogType = Enumeration.LogType.Warn
			};

            return _loggerDatabase.SaveLogAsync(log);
        }

		public Task<int> LogErrorAsync(string logMessage)
        {
			var log = new Log
            {
				LogMessage = logMessage,
				LogType = Enumeration.LogType.Error
			};

            return _loggerDatabase.SaveLogAsync(log);
        }

		public Task<int> DeleteLogAsync(Log item)
		{
			return _loggerDatabase.DeleteLogAsync(item);
		}
	}
}

