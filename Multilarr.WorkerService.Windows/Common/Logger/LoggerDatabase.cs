using System.Collections.Generic;
using System.Threading.Tasks;
using Multilarr.WorkerService.Windows.Common.Interfaces.Logger;
using Multilarr.WorkerService.Windows.Models;
using SQLite;

namespace Multilarr.WorkerService.Windows.Common.Logger
{
	public class LoggerDatabase : ILoggerDatabase
    {
		private readonly SQLiteAsyncConnection _database;

		public LoggerDatabase(string databasePath)
		{
			_database = new SQLiteAsyncConnection(databasePath);
			_database.CreateTableAsync<Log>().Wait();
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
	}
}

