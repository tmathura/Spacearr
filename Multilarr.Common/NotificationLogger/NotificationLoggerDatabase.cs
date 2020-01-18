using Multilarr.Common.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.NotificationLogger
{
	public class NotificationLoggerDatabase
    {
		private readonly SQLiteAsyncConnection _database;

		public NotificationLoggerDatabase(string databasePath)
		{
			_database = new SQLiteAsyncConnection(databasePath);
			_database.CreateTableAsync<NotificationLog>().Wait();
		}

		public async Task<List<NotificationLog>> GetLogsAsync()
		{
			return await _database.Table<NotificationLog>().ToListAsync();
		}

		public async Task<NotificationLog> GetLogAsync(int id)
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
	}
}

