using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Logger
{
    public class LoggerDatabase : ILoggerDatabase
    {
		private readonly SQLiteAsyncConnection _database;

		public LoggerDatabase(string databasePath)
		{
			_database = new SQLiteAsyncConnection(databasePath);
			_database.CreateTableAsync<Log>().Wait();
            _database.CreateTableAsync<NotificationLog>().Wait();
		}

		public async Task<List<Log>> GetLogsAsync()
		{
			return await _database.Table<Log>().ToListAsync();
		}

		public async Task<Log> GetLogAsync(int id)
		{
			return await _database.Table<Log>().Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<int> SaveLogAsync(Log item)
        {
            return await _database.InsertAsync(item);
        }

		public async Task<int> DeleteLogAsync(Log item)
		{
			return await _database.DeleteAsync(item);
		}

		#region Notifications
		
		public async Task<List<NotificationLog>> GetNotificationLogsAsync()
        {
            return await _database.Table<NotificationLog>().ToListAsync();
        }

        public async Task<NotificationLog> GetNotificationLogAsync(int id)
        {
            return await _database.Table<NotificationLog>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveLogAsync(NotificationLog item)
        {
            return await _database.InsertAsync(item);
        }

        public async Task<int> DeleteLogAsync(NotificationLog item)
        {
            return await _database.DeleteAsync(item);
        }

        #endregion
    }
}

